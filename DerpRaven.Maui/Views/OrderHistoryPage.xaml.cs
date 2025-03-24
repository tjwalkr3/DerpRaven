using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class OrderHistoryPage : ContentPage
{
	public OrderHistoryPage(OrderHistoryPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}