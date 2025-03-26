using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class CustomRequestPage : ContentPage
{
    public CustomRequestPage(CustomRequestPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}