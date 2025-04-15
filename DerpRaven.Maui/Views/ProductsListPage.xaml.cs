using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class ProductsListPage : Shell
{
    ProductsListPageViewModel _viewModel;
    public ProductsListPage(ProductsListPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;

        Navigated += OnNavigated;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.RefreshProductsView();
        }
    }

    private void OnNavigated(object sender, ShellNavigatedEventArgs e)
    {
        UpdateTitle(); // Update the title every time navigation happens
    }

    private void UpdateTitle()
    {
        if (this.CurrentItem is ShellItem shellItem)
        {
            var activeSection = shellItem.CurrentItem; // Get the active ShellSection
            this.Title = activeSection.Title; // This updates the parent Shell title
        }
    }
}