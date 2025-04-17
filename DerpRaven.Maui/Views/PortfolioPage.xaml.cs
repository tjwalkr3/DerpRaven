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
            _viewModel.SelectedTab = NavigationState.SelectedTab;
            _viewModel.SelectTab();
            NavigationState.SelectedTab = null;
        }


        if (_viewModel != null)
        {
            await _viewModel.RefreshPortfolioView();
        }
    }
}