using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui.Views;

public partial class OrderHistoryPage : ContentPage
{
    private readonly OrderHistoryPageViewModel _viewModel;
    public OrderHistoryPage(OrderHistoryPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnAppearing();
        await _viewModel.RefreshOrdersView();
    }
}