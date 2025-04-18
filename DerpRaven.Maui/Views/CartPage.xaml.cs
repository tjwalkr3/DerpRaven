using CommunityToolkit.Maui.Views;
using DerpRaven.Maui.Popups;
using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class CartPage : ContentPage
{
    private readonly CartPageViewModel _vm;
    public CartPage(CartPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _vm.PopulateCart();
    }

   
    public void OnDisplayPopupClicked(object sender, EventArgs e) {
        CartStorage _cart = new();
        decimal price = _cart.GetCartTotal();
        string PaymentUrl = "https://derpipose.github.io/Payment.html?price=" + price;
        var popup = new PaymentPopup(PaymentUrl);

        this.ShowPopup(popup);
    }
}