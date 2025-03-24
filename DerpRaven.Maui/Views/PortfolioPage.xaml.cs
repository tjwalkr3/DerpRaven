using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class PortfolioPage : ContentPage
{
	public PortfolioPage(PortfolioPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}