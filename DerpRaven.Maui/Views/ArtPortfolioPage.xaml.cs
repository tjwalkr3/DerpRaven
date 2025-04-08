using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class ArtPortfolioPage : ContentPage
{
    public ArtPortfolioPage(MainPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}