namespace DerpRaven.Maui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using CommunityToolkit.Mvvm.Input;

public partial class MainPageViewModel : ObservableObject
{
    public static class NavigationState
    {
        public static string SelectedTab;
    }

    [RelayCommand]
    private async Task NavigateToPlushiePortfolio()
    {
        NavigationState.SelectedTab = "PlushiePortfolio";
        await Shell.Current.GoToAsync("//PortfolioPage");

    }

    [RelayCommand]
    private async Task NavigateToArtPortfolio()
    {
        NavigationState.SelectedTab = "ArtPortfolio";
        await Shell.Current.GoToAsync("//PortfolioPage");
    }
}


