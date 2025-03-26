using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}