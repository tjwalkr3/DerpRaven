using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;


[QueryProperty(nameof(ProductIdQuery), "productId")]
public partial class ProductPageViewModel : ObservableObject
{
    public ObservableCollection<string> BreadcrumbItems { get; } = new();
    public string ProductIdQuery
    {
        set
        {
            if (int.TryParse(value, out var id)) ProductId = id;
        }
    }

    [ObservableProperty]
    private int productId;

    [ObservableProperty]
    ProductDto? _productDetails;

    [ObservableProperty]
    List<ImageDto> _images = [];

    [ObservableProperty]
    private int selectedQuantity;

    private readonly IImageHelpers _imageHelpers;
    private readonly IProductClient _productClient;

    [ObservableProperty]
    private bool isLoading;

    public ProductPageViewModel(IImageHelpers imageHelpers, IProductClient productClient)
    {
        _imageHelpers = imageHelpers;
        _productClient = productClient;
        SelectedQuantity = 1;

        BreadcrumbItems.Add("Products");
    }

    partial void OnProductIdChanged(int value)
    {
        Task.Run(async () => await RefreshSingleProductView());
    }

    public async Task RefreshSingleProductView()
    {
        IsLoading = true;
        try
        {
            ProductDetails = await _productClient.GetProductByIdAsync(ProductId);
            if (ProductDetails == null) return;

            OnPropertyChanged(nameof(QuantityOptions));
            await Task.Delay(100);
            SelectedQuantity = QuantityOptions.DefaultIfEmpty(1).Min();

            List<int> imageIds = ProductDetails?.ImageIds ?? [];
            if (imageIds.Count < 1) return;

            Images = await _imageHelpers.GetImageDtos(imageIds);
            Images = _imageHelpers.GetPaths(Images);
            await _imageHelpers.SaveListOfImages(Images);

            BreadcrumbItems.Clear();
            BreadcrumbItems.Add("Products");
            BreadcrumbItems.Add(ProductDetails?.Name ?? "Product Details");

        }
        finally
        {
            IsLoading = false;
        }
    }

    public List<int> QuantityOptions => Enumerable.Range(1, ProductDetails?.Quantity ?? 0).ToList();

    partial void OnProductDetailsChanged(ProductDto? value)
    {
        OnPropertyChanged(nameof(QuantityOptions));
    }

    //Add to cart will add the product to the cart
    [RelayCommand]
    private async Task AddToCartAsync(ProductDto product)
    {
        //if (product == null) return;
        //Debug.WriteLine($"Added {product.Name} to cart!");
        // Here we will add logic to update a cart collection
        //navigate to cart page
        await Shell.Current.GoToAsync($"///CartPage");
    }
}

