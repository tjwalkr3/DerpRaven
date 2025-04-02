using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;

namespace DerpRaven.Maui;

public partial class SplashScreen : ContentPage
{
    AppShell _appShell;
    public SplashScreen(AppShell appShell)
    {
        InitializeComponent();
        _appShell = appShell;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(3000).ContinueWith(t =>
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                var mainPageViewModel = new MainPageViewModel();
                if (Application.Current != null) Application.Current.Windows[0].Page = _appShell;
            });
        });
    }
}
