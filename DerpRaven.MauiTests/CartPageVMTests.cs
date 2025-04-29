using DerpRaven.Maui;
using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Authentication;
using Shouldly;
using NSubstitute;
using System.Collections.ObjectModel;

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
        _cartStorage.GetCartTotal().Returns(50.0m);

        _viewModel = new CartPageViewModel(_cartStorage, _keycloakClient);
    }

    [Test]
    public void PopulateCart_ShouldPopulateCartItemsAndUpdateRunningTotal()
    {
        // Act
        _viewModel.PopulateCart();
        //_viewModel.UpdateRunningTotal();

        // Assert
        _viewModel.CartItems.Count.ShouldBe(2);
        _viewModel.CartItems[0].Name.ShouldBe("Item1");
        _viewModel.CartItems[1].Name.ShouldBe("Item2");
        _viewModel.RunningTotal.ShouldBe(50.0m);
    }

    [Test]
    public void CheckIfItemsInCart_ShouldSetItemsInCartCorrectly()
    {
        // Act
        _viewModel.PopulateCart();
        _viewModel.CheckIfItemsInCart();
        // Assert
        _viewModel.ItemsInCart.ShouldBeTrue();
        _viewModel.NoItems.ShouldBeFalse();
    }

    [Test]
    public void CheckIfItemsInCart_ShouldSetNoItemsCorrectly_WhenNoItemsInCart()
    {
        // Arrange
        _cartStorage.GetCartItems().Returns(new List<CartItem>());
        _viewModel.PopulateCart();
        // Act
        _viewModel.CheckIfItemsInCart();
        // Assert
        _viewModel.ItemsInCart.ShouldBeFalse();
        _viewModel.NoItems.ShouldBeTrue();
    }

    [Test]
    public void CheckPlushiePresent_ShouldSetPlushiePresentCorrectly()
    {
        // Arrange
        var cartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1, Name = "Plushie", Price = 10.0m, Quantity = 1, ProductTypeId=1 }
        };
        _cartStorage.GetCartItems().Returns(cartItems);
        _viewModel.PopulateCart();
        // Act
        _viewModel.CheckPlushiePresent();
        // Assert
        _viewModel.PlushiePresent.ShouldBeTrue();
    }

    [Test]
    public void CheckPlushiePresent_ShouldSetPlushiePresentToFalse_WhenNoPlushie()
    {
        // Arrange
        var cartItems = new List<CartItem>
        {
            new CartItem { ProductId = 1, Name = "Item1", Price = 10.0m, Quantity = 1 }
        };
        _cartStorage.GetCartItems().Returns(cartItems);
        _viewModel.PopulateCart();
        // Act
        _viewModel.CheckPlushiePresent();
        // Assert
        _viewModel.PlushiePresent.ShouldBeFalse();
    }
}