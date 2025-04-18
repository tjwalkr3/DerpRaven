using DerpRaven.Maui.ViewModels;
using Microsoft.Maui.Controls;
using static DerpRaven.Maui.ViewModels.MainPageViewModel;
namespace DerpRaven.Maui.Views;

public partial class PortfolioPage : Shell
{
    PortfolioPageViewModel _viewModel;
    public PortfolioPage(PortfolioPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(NavigationState.SelectedTab))
        {
            var tabBar = this.Items.FirstOrDefault() as TabBar;

            var route = NavigationState.SelectedTab;

            ShellSection? targetTab = null;

            if (NavigationState.SelectedTab == "ArtPortfolio")
            {
                targetTab = tabBar?.Items.FirstOrDefault(i => i.Route == route);
            }
            else if (NavigationState.SelectedTab == "PlushiePortfolio")
            {
                targetTab = tabBar?.Items.FirstOrDefault(i => i.Route == route);
            }

            if (targetTab != null)
            {
                tabBar.CurrentItem = targetTab;
            }

            NavigationState.SelectedTab = null; // Clear it after use
        }


        if (_viewModel != null)
        {
            await _viewModel.RefreshPortfolioView();
        }
    }
}