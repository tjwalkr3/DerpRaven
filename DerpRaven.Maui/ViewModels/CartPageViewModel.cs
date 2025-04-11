using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;

public partial class CartPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CartItem> cartItems;

    [ObservableProperty]
    private decimal runningTotal;

    public CartPageViewModel()
    {
        cartItems = new ObservableCollection<CartItem>
        {
            new CartItem
            {
                Name = "Unicorn Squishy",
                ImageUrl = "unicornsquish.jpg",
                Quantity = 10,
                Price = 10.99M,
                ProductTypeId = 1
            },
            new CartItem
            {
                Name = "Horse Plushie",
                ImageUrl = "horsesnuggler.jpg",
                Quantity = 2,
                Price = 59.99M,
                ProductTypeId = 1
            },
            new CartItem
            {
                Name = "Emote",
                ImageUrl = "quincymad.png",
                Quantity = 1,
                Price = 14.99M,
                ProductTypeId = 2

            }
        };

        checkPlushiePresent();
    }

    [ObservableProperty]
    public bool plushiePresent = false;

    public void checkPlushiePresent()
    {
        foreach (var item in CartItems)
        {
            if (item.ProductTypeId == 1)
            {
                PlushiePresent = true;
                break;
            }
            else PlushiePresent = false;
        }
    }



    //[RelayCommand]
    //private void RemoveItem(CartItem item)
    //{
    //    cartItems.Remove(item);
    //    UpdateRunningTotal();
    //}

    //[RelayCommand]
    //private void Checkout()
    //{
    // Implement checkout logic here
    //}

    //private void UpdateRunningTotal()
    //{
    //    RunningTotal = cartItems.Sum(item => item.Total);
    //}
}

