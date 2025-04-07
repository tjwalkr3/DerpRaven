using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Maui.Views;

[QueryProperty(nameof(ProductId), "productId")]
public partial class ProductPage : ContentPage
{
    private readonly ProductsListPageViewModel _viewModel;
    private int _productId;
    public ObservableCollection<int> Quantities { get; set; }

    private int _selectedQuantity;

    public int SelectedQuantity
    {
        get => _selectedQuantity;
        set
        {
            _selectedQuantity = value;
            OnPropertyChanged();
        }
    }

    public int ProductId
    {
        get => _productId;
        set
        {
            _productId = value;
            LoadProductDetails(value);
        }
    }

    public ProductPage(ProductsListPageViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;

        Quantities = new ObservableCollection<int>(Enumerable.Range(1, 10));

        QuantityPicker.ItemsSource = Quantities;

        QuantityPicker.SelectedItem = Quantities.First();
    }

    private void LoadProductDetails(int productId)
    {
        // Find the product with the matching ID from the ViewModel's Products collection
        var selectedProduct = _viewModel.Products.FirstOrDefault(p => p.Id == productId);

        if (selectedProduct != null)
        {
            // Bind the selected product to the page
            BindingContext = selectedProduct;
        }

        ProductImageCarousel.ItemsSource = new List<string> { selectedProduct.ImageUrl };
    }
}
