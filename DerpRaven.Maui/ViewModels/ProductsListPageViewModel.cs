using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DerpRaven.Maui.ViewModels;

public partial class ProductsListPageViewModel : ObservableObject
{
    public ObservableCollection<Product> Products { get; private set; }

    public ProductsListPageViewModel()
    {
        Products = new ObservableCollection<Product>
        {
            new Product
            {
                Id = 1, // Unique identifier for the product
                Name = "Unicorn Squishy",
                ImageUrl = "unicornsquish.png",
                Description = "A cute unicorn stress squishy",
                Price = 10.99,
                Quantity = 10
            },
            new Product
            {
                Id = 2, // Unique identifier for the product
                Name = "Horse Plushie",
                ImageUrl = "horsesnuggler.png",
                Description = "A cute and cuddly horse plushie",
                Price = 59.99
            },
            new Product
            {
                Id = 3, // Unique identifier for the product
                Name = "Emote",
                ImageUrl = "quincymad.png",
                Description = "A custom emote page",
                Price = 14.99
            }
        };
    }

    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product == null) return;
        Debug.WriteLine($"Added {product.Name} to cart!");
        // Here you could add logic to update a cart collection
    }

    [RelayCommand]
    private async Task NavigateToProduct(Product product)
    {
        if (product == null) return;
        await Shell.Current.GoToAsync($"///ProductPage?productId={product.Id}");
    }
}
public class Product
{
    public int Id { get; set; } // Unique identifier for the product
    public string Name { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
    public string ImageUrl { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
    public string Description { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
    public double Price { get; set; }
    public int Quantity { get; set; } = 1; // Default quantity to 1 for the product
}