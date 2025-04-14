﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.Authentication;
using Duende.IdentityModel.OidcClient;
namespace DerpRaven.Maui.ViewModels;

public partial class AppShellViewModel : ObservableObject
{
    private readonly IKeycloakClient _oktaClient;
    private LoginResult _authenticationData = default!;

    public AppShellViewModel(IKeycloakClient oktaClient)
    {
        _oktaClient = oktaClient;
        oktaClient.IdentityTokenChanged += OnIdentityTokenChanged;
        OnIdentityTokenChanged();
    }

    [ObservableProperty]
    private bool isLoggedIn = false;

    private void OnIdentityTokenChanged()
    {
        IsLoggedIn = !string.IsNullOrEmpty(_oktaClient.IdentityToken);
    }

    public async Task TryToLogin(ToolbarItem loginToolbarItem)
    {
        if (loginToolbarItem.Text == "Login")
        {
            var loginResult = await _oktaClient.LoginAsync();
            if (!loginResult.IsError)
            {
                _authenticationData = loginResult;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
            UpdateLoginLogoutButton(loginToolbarItem);
        }
        else if (!string.IsNullOrEmpty(_oktaClient.IdentityToken))
        {
            var logOut = await _oktaClient.LogoutAsync(_oktaClient.IdentityToken);
            if (logOut.IsError)
            {
                await Shell.Current.DisplayAlert("Error", logOut.ErrorDescription, "OK");
            }
            UpdateLoginLogoutButton(loginToolbarItem);
        }
    }

    private void UpdateLoginLogoutButton(ToolbarItem loginToolbarItem)
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
