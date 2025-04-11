using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class ArtProductsListPage : ContentPage
{
	public ArtProductsListPage(ProductsListPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}