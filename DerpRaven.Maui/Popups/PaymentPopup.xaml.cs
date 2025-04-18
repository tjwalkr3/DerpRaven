using CommunityToolkit.Maui.Views;

namespace DerpRaven.Maui.Popups;

public partial class PaymentPopup : Popup
{
    public PaymentPopup()
    {
        InitializeComponent();
    }
    void OnOKButtonClicked(object? sender, EventArgs e) => Close();
}