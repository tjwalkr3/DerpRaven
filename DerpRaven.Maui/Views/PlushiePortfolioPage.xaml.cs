using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class PlushiePortfolioPage : ContentPage
{
    public PlushiePortfolioPage(PortfolioPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}