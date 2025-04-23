using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using NSubstitute;
using Shouldly;

namespace DerpRaven.MauiTests;

public class ProductPageVMTests
{
    private ProductPageViewModel? _viewModel;
    private IKeycloakClient _mockKeycloakClient;
    private ICartStorage _mockCartStorage;

    [SetUp]
    public void Setup()
    {
        var productClient = Substitute.For<IProductClient>();
        var imageHelpers = Substitute.For<IImageHelpers>();
        _mockCartStorage = Substitute.For<ICartStorage>();
        _mockKeycloakClient = Substitute.For<IKeycloakClient>();

        _viewModel = new ProductPageViewModel(imageHelpers, productClient, _mockKeycloakClient, _mockCartStorage);
    }

    [Test]
    public void OnIdentityTokenChanged_ShouldUpdateCartButtonLoggedIn()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns("valid_token");

        // Act
        _viewModel?.populateCartButton();

        // Assert
        _viewModel?.IsSignedIn.ShouldBeTrue();
        _viewModel?.CartButtonText.ShouldBe("Add to cart");
    }

    [Test]
    public void OnIdentityTokenChanged_ShouldUpdateCartButtonLoggedOut()
    {
        // Arrange
        _mockKeycloakClient.IdentityToken.Returns((string?)null);

        // Act
        _viewModel?.populateCartButton();

        // Assert
        _viewModel?.IsSignedIn.ShouldBeFalse();
        _viewModel?.CartButtonText.ShouldBe("Login to add to cart");
    }
}
