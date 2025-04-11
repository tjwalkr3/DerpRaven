using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class PlushieProductsListPage : ContentPage
{
	public PlushieProductsListPage(ProductsListPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}