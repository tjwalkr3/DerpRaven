﻿namespace DerpRaven.Maui.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

public partial class MainPageViewModel : ObservableObject
{
    public ObservableCollection<Emote> Emotes { get; private set; }
    public ObservableCollection<Emote> Plushies { get; private set; }


    public MainPageViewModel()
    {
        Emotes = new ObservableCollection<Emote>
        {
            new Emote
            {
                Name = "Quincy Mad Emote",
                ImageUrl = "quincymad.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Quincy Meh Emote",
                ImageUrl = "quincymeh.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Quincy Shy Emote",
                ImageUrl = "quincyshy.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Quincy Upset Emote",
                ImageUrl = "quincyupset.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Ramus Angry Emote",
                ImageUrl = "ramusangry.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Ramus Happy Emote",
                ImageUrl = "ramushappy.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Ramus Sad Emote",
                ImageUrl = "ramussad.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Ramus Tired Emote",
                ImageUrl = "ramustired.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Roxanne Grumpy Emote",
                ImageUrl = "roxannegrumpy.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Roxanne Happy Emote",
                ImageUrl = "roxannehappy.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Roxanne Laughing Emote",
                ImageUrl = "roxannelaughing.png",
                Description = "A custom emote for a client"
            },
            new Emote
            {
                Name = "Roxanne Seducing Emote",
                ImageUrl = "roxanneseducing.png",
                Description = "A custom emote for a client"
            }
        };

        Plushies = new ObservableCollection<Emote>
        {
            new Emote
            {
                Name = "Derp Squid Squish",
                ImageUrl = "derpsquid.png",
                Description = "A custom plush for a client"
            },
            new Emote
            {
                Name = "Flower Turtle",
                ImageUrl = "flowerturtle.png",
                Description = "A custom plush for a client"
            },
            new Emote
            {
                Name = "Dragon Plush",
                ImageUrl = "dragonplush.png",
                Description = "A plush for a client"
            },
            new Emote
            {
                Name = "Puffer Squish",
                ImageUrl = "puffersquish.png",
                Description = "A custom plush for a client"
            },
            new Emote
            {
                Name = "Unicorn Squish",
                ImageUrl = "unicornsquish.png",
                Description = "A custom plush for a client"
            },
            new Emote
            {
                Name = "Horse Snuggler",
                ImageUrl = "horsesnuggler.png",
                Description = "A horse snuggler"
            }
        };
    }
}

public class Emote
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
