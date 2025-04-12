using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;


[QueryProperty(nameof(ProductId), "productId")]
public partial class ProductPageViewModel : ObservableObject
{
    //Ghost Data
    ProductDto ghost_product = new ProductDto {
        Name = "Unicorn Squishy",
        ImageIds = [1,2,3],
        Description = "A cute unicorn stress squishy",
        Price = 10.99m,
        Quantity = 10
    };
    List<ImageDto> ghost_images = new List<ImageDto>() {
        new ImageDto {
            Id = 1,
            Path = "unicornsquish.jpg",
            Alt = "Unicorn Squishy"
        },new ImageDto {
            Id = 2,
            Path = "derpsquid.jpg",
            Alt = "Unicorn Squishy"
        },new ImageDto {
            Id = 3,
            Path = "puffersquish.jpg",
            Alt = "Unicorn Squishy"
        }
    };


    public List<int> QuantityOptions => Enumerable.Range(1, ProductDetails?.Quantity ?? 0).ToList();


    [ObservableProperty]
    private int productId;

    [ObservableProperty]
    ProductDto _productDetails;

    [ObservableProperty]
    List<ImageDto> _images = new();

    [ObservableProperty]
    private int selectedQuantity;

    public ProductPageViewModel() {
        _productDetails = GetProductInfo();
       GetImages();
        SelectedQuantity = 1;
    }

    public ProductDto GetProductInfo() {
        // This is where you would typically fetch the product details from a service
        // For now, we'll just return the ghost data
        return ghost_product;
    }
    public void GetImages() {
        // This is where you would typically fetch the product details from a service
        // For now, we'll just return the ghost data
        Images.Clear();
        foreach(var image in ghost_images) {
            Images.Add(image);
        }
    }

    partial void OnProductDetailsChanged(ProductDto value) {
        OnPropertyChanged(nameof(QuantityOptions));
    }


    //Add to cart will add the product to the cart
    [RelayCommand]
    private async Task AddToCartAsync(ProductDto product) {
        //if (product == null) return;
        //Debug.WriteLine($"Added {product.Name} to cart!");
        // Here we will add logic to update a cart collection
        //navigate to cart page
        await Shell.Current.GoToAsync($"///CartPage");
    }


}

