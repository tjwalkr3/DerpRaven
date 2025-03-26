using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}