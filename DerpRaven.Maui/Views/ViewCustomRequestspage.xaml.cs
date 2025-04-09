using DerpRaven.Maui.ViewModels;

namespace DerpRaven.Maui.Views;

public partial class ViewCustomRequestsPage : ContentPage
{
    public ViewCustomRequestsPage(ViewCustomRequestsPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}