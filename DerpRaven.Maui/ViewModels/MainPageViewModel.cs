namespace DerpRaven.Maui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Dtos;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

public partial class MainPageViewModel : ObservableObject
{
    public ObservableCollection<ImageDto> Art { get; private set; }
    public ObservableCollection<ImageDto> Plushies { get; private set; }


    public MainPageViewModel()
    {
        Art = new ObservableCollection<ImageDto>
        {
            new ImageDto
            {
                Id = 1,
                Alt = "Quincy Mad Emote",
                Path = "quincymad.png"
            },
            new ImageDto
            {
                Id = 2,
                Alt = "Quincy Meh Emote",
                Path = "quincymeh.png"
            },
            new ImageDto
            {
                Id = 3,
                Alt = "Quincy Shy Emote",
                Path = "quincyshy.png"
            },
            new ImageDto
            {
                Id = 4,
                Alt = "Quincy Upset Emote",
                Path = "quincyupset.png"
            },
            new ImageDto
            {
                Id = 5,
                Alt = "Ramus Angry Emote",
                Path = "ramusangry.png"
            },
            new ImageDto
            {
                Id = 6,
                Alt = "Ramus Happy Emote",
                Path = "ramushappy.png"
            },
            new ImageDto
            {
                Id = 7,
                Alt = "Ramus Sad Emote",
                Path = "ramussad.png"
            },
            new ImageDto
            {
                Id = 8,
                Alt = "Ramus Tired Emote",
                Path = "ramustired.png"
            },
            new ImageDto
            {
                Id = 9,
                Alt = "Roxanne Grumpy Emote",
                Path = "roxannegrumpy.png"
            },
            new ImageDto
            {
                Id = 10,
                Alt = "Roxanne Happy Emote",
                Path = "roxannehappy.png"
            },
            new ImageDto
            {
                Id = 11,
                Alt = "Roxanne Laughing Emote",
                Path = "roxannelaughing.png"
            },
            new ImageDto
            {
                Id = 12,
                Alt = "Roxanne Seducing Emote",
                Path = "roxanneseducing.png"
            }
        };

        Plushies = new ObservableCollection<ImageDto>
        {
            new ImageDto
            {
                Id = 13,
                Alt = "Derp Squid Squish",
                Path = "derpsquid.png"
            },
            new ImageDto
            {
                Id = 14,
                Alt = "Flower Turtle",
                Path = "flowerturtle.png"
            },
            new ImageDto
            {
                Id = 15,
                Alt = "Dragon Plush",
                Path = "dragonplush.png"
            },
            new ImageDto
            {
                Id = 16,
                Alt = "Puffer Squish",
                Path = "puffersquish.png"
            },
            new ImageDto
            {
                Id = 17,
                Alt = "Unicorn Squish",
                Path = "unicornsquish.png"
            },
            new ImageDto
            {
                Id = 18,
                Alt = "Horse Snuggler",
                Path = "horsesnuggler.png"
            }

        };
    }

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


