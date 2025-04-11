using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class ProductsListPage : Shell
{
    public ProductsListPage(ProductsListPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}