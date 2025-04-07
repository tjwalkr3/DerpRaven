using DerpRaven.Shared.Authentication;
using Duende.IdentityModel.OidcClient;

namespace DerpRaven.Maui;

public partial class AppShell : Shell
{
    KeycloakClient _oktaClient;
    private LoginResult _authenticationData = default!;
    public AppShell(KeycloakClient oktaClient)
    {
        InitializeComponent();
        _oktaClient = oktaClient;
    }

    public async void OnButtonClicked(object? sender, EventArgs e)
    {
        var loginToolbarItem = this.ToolbarItems.FirstOrDefault(t => t.Text == "Login" || t.Text.Contains("Logout"));
        if (loginToolbarItem == null) return;
        if (loginToolbarItem.Text == "Login")
        {
            var loginResult = await _oktaClient.LoginAsync();
            if (!loginResult.IsError)
            {
                _authenticationData = loginResult;
            }
            else
            {
                await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
            _oktaClient.IdentityToken = loginResult.IdentityToken;
            UpdateLoginLogoutButton();
        }
        else if (!string.IsNullOrEmpty(_oktaClient.IdentityToken))
        {
            var logOut = await _oktaClient.LogoutAsync(_oktaClient.IdentityToken);
            if (logOut.IsError)
            {
                await DisplayAlert("Error", logOut.ErrorDescription, "OK");
            }
            _oktaClient.IdentityToken = string.Empty;
            UpdateLoginLogoutButton();
        }
    }

    private void UpdateLoginLogoutButton()
    {
        var loginToolbarItem = this.ToolbarItems.FirstOrDefault(t => t.Text == "Login" || t.Text.Contains("Logout"));
        if (loginToolbarItem != null)
        {
            if (!string.IsNullOrEmpty(_oktaClient.IdentityToken))
            {
                string email = _authenticationData.User.FindFirst("email")?.Value ?? "unknown";
                loginToolbarItem.Text = $"Logout {email}";
            }
            else
            {
                loginToolbarItem.Text = "Login";
            }
        }
    }
}
