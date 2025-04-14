using DerpRaven.Maui.ViewModels;
namespace DerpRaven.Maui;

public partial class AppShell : Shell
{
    private readonly AppShellViewModel _viewModel;

    public AppShell(AppShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    public async void OnButtonClicked(object? sender, EventArgs e)
    {
        var loginToolbarItem = this.ToolbarItems.FirstOrDefault(t => t.Text == "Login" || t.Text.Contains("Logout"));
        if (loginToolbarItem == null) return;
        await _viewModel.TryToLogin(loginToolbarItem);
    }
}
