using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DerpRaven.Maui;
using DerpRaven.Maui.Popups;
using CommunityToolkit.Maui.Views;

namespace DerpRaven.Maui.ViewModels;

public partial class CartPageViewModel : ObservableObject
{

    private readonly ICartStorage _cartStorage;

    [ObservableProperty]
    private ObservableCollection<CartItem> cartItems = [];

    [ObservableProperty]
    private decimal runningTotal = 0.00m;


    public CartPageViewModel(ICartStorage cartStorage)
    {
        _cartStorage = cartStorage;
        PopulateCart();
        CheckPlushiePresent();
        UpdateRunningTotal();
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
    }

    [ObservableProperty]
    public bool plushiePresent = false;

    public void CheckPlushiePresent()
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



    [RelayCommand]
    private void RemoveItem(CartItem item)
    {
        _cartStorage.RemoveCartItem(item);
        UpdateRunningTotal();
        PopulateCart();
    }

    [RelayCommand]
    private void Checkout() {
        // Implement checkout logic here  
        var popup = new PaymentPopup();

        // Show popup using the current visible page  
        var currentPage = Application.Current.MainPage;

        // Corrected the issue by using the correct type cast and accessing the MainPage directly  
        if (currentPage is Page page) {
            page.ShowPopup(popup);
        }
    }

    private void UpdateRunningTotal()
    {
        RunningTotal = CartItems.Sum(item => item.Quantity * item.Price);
    }
}

