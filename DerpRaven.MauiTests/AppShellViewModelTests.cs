namespace DerpRaven.MauiTests;

using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Authentication;
using Duende.IdentityModel.OidcClient;
using Microsoft.Maui.Controls;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Security.Claims;

public class AppShellViewModelTests
{
    private IKeycloakClient _mockKeycloakClient;
    private IUserStorage _mockUserStorage;
    private AppShellViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockKeycloakClient = Substitute.For<IKeycloakClient>();
        _mockUserStorage = Substitute.For<IUserStorage>();
        _mockUserStorage.SetEmail(Arg.Any<string>());
        _viewModel = new AppShellViewModel(_mockKeycloakClient, _mockUserStorage);
    }

    [Test]
    public void OnIdentityTokenChanged_ShouldUpdateIsLoggedIn()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns("valid_token");

        // Act
        _viewModel.OnIdentityTokenChanged();

        // Assert
        _viewModel.IsLoggedIn.ShouldBeTrue();
    }

    [Test]
    public void UpdateLoginLogoutButton_ShouldSetLogoutTextWhenLoggedIn()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns("valid_token");
        var toolbarItem = new ToolbarItem();
        var loginResult = Substitute.For<LoginResult>();
        loginResult.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("email", "test@example.com") })));

        _viewModel.GetType().GetField("_authenticationData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_viewModel, loginResult);

        // Act
        _viewModel.UpdateLoginLogoutButton(toolbarItem);

        // Assert
        toolbarItem.Text.ShouldBe("Logout");
    }

    [Test]
    public async Task TryToLogin_ShouldLogoutSuccessfully()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns("valid_token");
        _viewModel.OnIdentityTokenChanged();
        var logoutResult = Substitute.For<LogoutResult>();
        logoutResult.IsError.Returns(false);

        _mockKeycloakClient.LogoutAsync("valid_token").Returns(logoutResult);
        var toolbarItem = new ToolbarItem { Text = "Logout" };

        // Act
        await _viewModel.TryToLogin(toolbarItem);

        // Assert
        toolbarItem.Text.ShouldBe("Login");
    }

    [Test]
    public async Task TryToLogin_ShouldLoginSuccessfully()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns("valid_token");
        _viewModel.OnIdentityTokenChanged();
        var loginResult = Substitute.For<LoginResult>();
        loginResult.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("email", "test@example.com") })));
        _viewModel.GetType().GetField("_authenticationData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_viewModel, loginResult);

        _mockKeycloakClient.LoginAsync().Returns(loginResult);
        var toolbarItem = new ToolbarItem { Text = "Login" };

        // Act
        await _viewModel.TryToLogin(toolbarItem);

        // Assert
        toolbarItem.Text.ShouldBe("Logout");
    }

    [Test]
    public void UpdateLoginLogoutButton_ShouldSetLoginTextWhenLoggedOut()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns((string?)null);
        var toolbarItem = new ToolbarItem();

        // Act
        _viewModel.UpdateLoginLogoutButton(toolbarItem);

        // Assert
        toolbarItem.Text.ShouldBe("Login");
    }
}
