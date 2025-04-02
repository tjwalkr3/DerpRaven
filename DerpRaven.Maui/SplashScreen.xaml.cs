using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;

namespace DerpRaven.Maui;

public partial class SplashScreen : ContentPage
{
    public SplashScreen()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(1500).ContinueWith(t =>
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                var mainPageViewModel = new MainPageViewModel();
                if (Application.Current != null) Application.Current.Windows[0].Page = new AppShell();
            });
        });
    }
}
