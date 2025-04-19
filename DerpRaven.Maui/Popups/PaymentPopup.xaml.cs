using CommunityToolkit.Maui.Views;

namespace DerpRaven.Maui.Popups;


public partial class PaymentPopup : Popup
{
    bool loaded = false;
    public string PaymentUrl { get; set; }
    public PaymentPopup(string Url)
    {
        PaymentUrl = Url;
        //_cart = new();
        InitializeComponent();
        BindingContext = this;

    }


    public void WebMessageReceived(object sender, WebNavigatingEventArgs e)
    {
        //if (loaded) {
        //    _cart.AddNonce(e.Url);
        //    //e.Cancel = true;
        //    //await Navigation.PopAsync();
        //} else {
        //    loaded = true;
        //}

    }
    void OnClosePopupClicked(object? sender, EventArgs e) => Close();

}