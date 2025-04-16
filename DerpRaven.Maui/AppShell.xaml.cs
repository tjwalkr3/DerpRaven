using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;
namespace DerpRaven.Maui;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;

    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        Routing.RegisterRoute("ProductPage", typeof(ProductPage));
    }

    public async void OnButtonClicked(object? sender, EventArgs e)
    {
        var loginToolbarItem = this.ToolbarItems.FirstOrDefault(t => t.Text == "Login" || t.Text.Contains("Logout"));
        if (loginToolbarItem == null) return;
        await _viewModel.TryToLogin(loginToolbarItem);
    }

}
