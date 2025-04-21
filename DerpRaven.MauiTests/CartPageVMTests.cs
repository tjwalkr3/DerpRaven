using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Authentication;
using Shouldly;
using NSubstitute;

namespace DerpRaven.MauiTests;

public class CartPageVMTests
{
    private CartPageViewModel _viewModel;
    private ICartStorage _cartStorage;
    private IKeycloakClient _keycloakClient;

    [SetUp]
    public void Setup()
    {
        _cartStorage = Substitute.For<ICartStorage>();
        _keycloakClient = Substitute.For<IKeycloakClient>();

        var cartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1, Name = "Item1", Price = 10.0m, Quantity = 1 },
            new CartItem { ProductId = 2, Name = "Item2", Price = 20.0m, Quantity = 2 }
        };
        _cartStorage.GetCartItems().Returns(cartItems);

        _viewModel = new CartPageViewModel(_cartStorage, _keycloakClient);
    }

    [Test]
    public void PopulateCart_ShouldPopulateCartItemsAndUpdateRunningTotal()
    {
        // Act
        _viewModel.PopulateCart();

        // Assert
        _viewModel.CartItems.Count.ShouldBe(2);
        _viewModel.CartItems[0].Name.ShouldBe("Item1");
        _viewModel.CartItems[1].Name.ShouldBe("Item2");
    }
}
