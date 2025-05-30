﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui.ViewModels;


[QueryProperty(nameof(ProductIdQuery), "productId")]
public partial class ProductPageViewModel : ObservableObject
{
    public string ProductIdQuery
    {
        set
        {
            if (int.TryParse(value, out var id)) ProductId = id;
        }
    }

    [ObservableProperty]
    public bool isSignedIn;

    [ObservableProperty]
    private int productId;

    [ObservableProperty]
    ProductDto? _productDetails;

    [ObservableProperty]
    List<ImageDto> _images = [];

    [ObservableProperty]
    private int selectedQuantity;

    [ObservableProperty]
    string _cartButtonText = string.Empty;

    private readonly IKeycloakClient _oktaClient;
    private readonly IImageClient _imageClient;
    private readonly IProductClient _productClient;
    private readonly ICartStorage _cartStorage;


    [ObservableProperty]
    private bool isLoading;

    public List<int> QuantityOptions => Enumerable.Range(1, ProductDetails?.Quantity ?? 0).ToList();

    public ProductPageViewModel(IImageClient imageClient, IProductClient productClient, IKeycloakClient keycloakClient, ICartStorage cartStorage)
    {
        _cartStorage = cartStorage;
        _oktaClient = keycloakClient;
        _imageClient = imageClient;
        _productClient = productClient;
        SelectedQuantity = 1;
        keycloakClient.IdentityTokenChanged += IdentityTokenHasChanged;
    }

    public void populateCartButton()
    {
        IsSignedIn = !string.IsNullOrEmpty(_oktaClient.IdentityToken);
        //check if logged in
        if (string.IsNullOrEmpty(_oktaClient.IdentityToken))
        {
            CartButtonText = "Login to add to cart";
        }
        else
        {
            CartButtonText = "Add to cart";
        }
    }

    partial void OnProductIdChanged(int value)
    {
        Task.Run(async () => await RefreshSingleProductView());
    }

    public async void IdentityTokenHasChanged()
    {
        await RefreshSingleProductView();
    }

    public async Task<List<ImageDto>> GetImageDtos(List<int> imageIds) => await _imageClient.GetImageInfoManyAsync(imageIds);

    public async Task RefreshSingleProductView()
    {
        IsLoading = true;
        try
        {
            ProductDetails = await _productClient.GetProductByIdAsync(ProductId);
            if (ProductDetails == null) return;

            OnPropertyChanged(nameof(QuantityOptions));
            SelectedQuantity = QuantityOptions.Min();

            List<int> imageIds = ProductDetails?.ImageIds ?? [];
            if (imageIds.Count < 1) return;


            Images = await GetImageDtos(imageIds);
            populateCartButton();
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnProductDetailsChanged(ProductDto? value)
    {
        OnPropertyChanged(nameof(QuantityOptions));
        OnPropertyChanged(nameof(SelectedQuantity));
    }

    [RelayCommand]
    private async Task AddToCart()
    {
        await Shell.Current.GoToAsync("//ProductsListPage");
        _cartStorage.AddCartItem(ProductDetails, SelectedQuantity);
        await Shell.Current.GoToAsync($"///CartPage");
    }
}

