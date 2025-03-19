using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
