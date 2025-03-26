using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class CartPage : ContentPage
{
    public CartPage(CartPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}