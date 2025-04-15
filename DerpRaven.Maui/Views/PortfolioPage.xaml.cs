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

        Navigated += OnNavigated;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel != null)
        {
            await _viewModel.RefreshPortfolioView();
        }
    }

    private void OnNavigated(object sender, ShellNavigatedEventArgs e)
    {
        UpdateTitle(); // Update the title every time navigation happens
    }

    private void UpdateTitle()
    {
        if (this.CurrentItem is ShellItem shellItem)
        {
            var activeSection = shellItem.CurrentItem; // Get the active ShellSection
            this.Title = activeSection.Title; // This updates the parent Shell title
        }
    }
}