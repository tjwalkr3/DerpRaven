using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DerpRaven.Shared.Authentication;

namespace DerpRaven.Maui.ViewModels;

public partial class CartPageViewModel : ObservableObject
{

    private readonly ICartStorage _cartStorage;
    private readonly IKeycloakClient _keycloakClient;

    [ObservableProperty]
    private ObservableCollection<CartItem> cartItems = [];

    [ObservableProperty]
    private string emailContact = string.Empty;

    [ObservableProperty]
    private string shippingAddress = string.Empty;

    [ObservableProperty]
    private decimal runningTotal = 0.00m;

    [ObservableProperty]
    bool itemsInCart = false;

    [ObservableProperty]
    bool noItems = false;

    public CartPageViewModel(ICartStorage cartStorage, IKeycloakClient keycloakClient)
    {
        _cartStorage = cartStorage;
        _keycloakClient = keycloakClient;
        PopulateCart();
        CheckPlushiePresent();
    }

    public void PopulateCart()
    {
        // Get the cart items from storage
        var cartItemsFromStorage = _cartStorage.GetCartItems();
        // Clear the cart items collection
        CartItems = new ObservableCollection<CartItem>();
        // Add the items from storage to the cart items collection
        foreach (var item in cartItemsFromStorage)
        {
            CartItems.Add(item);
        }
        // Update the running total
        UpdateRunningTotal();
        CheckPlushiePresent();
        CheckIfItemsInCart();
    }

    [ObservableProperty]
    public bool plushiePresent = false;

    public void CheckIfItemsInCart()
    {
        if (CartItems.Count >= 1)
        {
            ItemsInCart = true;
            NoItems = false;
        }
        else
        {
            ItemsInCart = false;
            NoItems = true;
        }
    }

    public void CheckPlushiePresent()
    {
        if (CartItems.Count == 0)
        {
            PlushiePresent = false;
        }
        else
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
    }



    [RelayCommand]
    private void RemoveItem(CartItem item)
    {
        _cartStorage.RemoveCartItem(item);
        UpdateRunningTotal();
        PopulateCart();
    }

    [RelayCommand]
    private async Task Checkout()
    {
        // Implement checkout logic here
        // Treating the checkout as successful
        if (PlushiePresent)
        {
            await _cartStorage.CheckOut(ShippingAddress, EmailContact);
        }
        else
        {
            await _cartStorage.CheckOut("", EmailContact);
        }
        PopulateCart();
    }

    private void UpdateRunningTotal()
    {
        RunningTotal = _cartStorage.GetCartTotal();
    }
}

