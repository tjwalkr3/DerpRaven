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
        var popup = new PaymentPopup();

        this.ShowPopup(popup);
    }
}