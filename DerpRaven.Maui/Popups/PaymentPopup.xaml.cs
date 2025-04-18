using CommunityToolkit.Maui.Views;

namespace DerpRaven.Maui.Popups;

public partial class PaymentPopup : Popup {
    bool loaded = false;
    public PaymentPopup()
	{
		InitializeComponent();
	}

    public async void WebMessageReceived(object sender, WebNavigatingEventArgs e) {
        if (loaded) {
            cart.AddNonce(e.Url);
            e.Cancel = true;
            if (SendOrder != null)
                await SendOrder();
            await Navigation.PopAsync();
        } else {
            loaded = true;
        }
    }
    void OnClosePopupClicked(object? sender, EventArgs e) => Close();
}