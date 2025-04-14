using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class ProductsListPage : Shell
{
    ProductsListPageViewModel _viewModel;
    public ProductsListPage(ProductsListPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.RefreshProductsView();
        }
    }
}