using DerpRaven.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;

namespace DerpRaven.MauiTests;

public class CartItemTests
{
    [Test]
    public void Test1()
    {
        // Arrange
        CartItem cartItem1 = new() { Name = "name", ProductId = 3, ImageUrl = "http://com.com", Quantity = 3, Price=3.75m, ProductTypeId = 2 };
        CartItem cartItem2 = new() { Name = "name", ProductId = 3, ImageUrl = "http://com.com", Quantity = 3, Price = 3.75m, ProductTypeId = 2 };
        
        // Act
        bool areEqual = cartItem1.Equals(cartItem2);

        // Assert
        areEqual.ShouldBeTrue();
    }
}
