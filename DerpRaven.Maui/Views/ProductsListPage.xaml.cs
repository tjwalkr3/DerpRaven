using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class ProductsListPage : ContentPage
{
    public ProductsListPage(ProductsListPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}