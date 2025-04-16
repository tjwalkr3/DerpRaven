using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

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
    private readonly IImageHelpers _imageHelpers;
    private readonly IProductClient _productClient;
    private readonly ICartStorage _cartStorage;


    public ProductPageViewModel(IImageHelpers imageHelpers, IProductClient productClient, IKeycloakClient keycloakClient, ICartStorage cartStorage)
    {
        _cartStorage = cartStorage;
        _oktaClient = keycloakClient;
        _imageHelpers = imageHelpers;
        _productClient = productClient;
        SelectedQuantity = 1;
    }

    private void populateCartButton() {
        IsSignedIn = !string.IsNullOrEmpty(_oktaClient.IdentityToken);
        //check if logged in
        if (string.IsNullOrEmpty(_oktaClient.IdentityToken)) {
            CartButtonText = "Login to add to cart";
        } else {
            CartButtonText = "Add to cart";
        }
    }

    partial void OnProductIdChanged(int value)
    {
        Task.Run(async () => await RefreshSingleProductView());
    }

    public async Task RefreshSingleProductView()
    {
        ProductDetails = await _productClient.GetProductByIdAsync(ProductId);
        if (ProductDetails == null) return;

        List<int> imageIds = ProductDetails?.ImageIds ?? [];
        if (imageIds.Count < 1) return;

        Images = await _imageHelpers.GetImageDtos(imageIds);
        Images = _imageHelpers.GetPaths(Images);
        await _imageHelpers.SaveListOfImages(Images);
        populateCartButton();
    }

    public List<int> QuantityOptions => Enumerable.Range(1, ProductDetails?.Quantity ?? 0).ToList();

    partial void OnProductDetailsChanged(ProductDto? value)
    {
        OnPropertyChanged(nameof(QuantityOptions));
    }

    //Add to cart will add the product to the cart
    [RelayCommand]
    private async Task AddToCart()
    {
        //add to cart
        _cartStorage.AddCartItem(ProductDetails);
        await Shell.Current.GoToAsync($"///CartPage");
    }


}

