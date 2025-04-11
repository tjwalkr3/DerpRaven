using DerpRaven.Maui.ViewModels;
using Microsoft.Maui.Controls;
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
        if (_viewModel != null)
        {
            await _viewModel.RefreshPortfolioView();
        }
    }
}