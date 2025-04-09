using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class CreateCustomRequestPage : ContentPage
{
	public CreateCustomRequestPage(CreateCustomRequestPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}