using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class PaymentPage : ContentPage
{
    public PaymentPage(PaymentPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}