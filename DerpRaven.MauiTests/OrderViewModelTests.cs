using DerpRaven.Maui.ViewModels;
using DerpRaven.Shared.Dtos;
using Shouldly;

namespace DerpRaven.MauiTests;

public class OrderViewModelTests
{
    private OrderViewModel _orderViewModel;

    [SetUp]
    public void Setup()
    {
        // Create a new OrderDto
        var order = new OrderDto
        {
            Id = 1,
            Address = "123 Test Street",
            Email = "test@example.com",
            OrderDate = new DateTime(2025, 4, 21),
            UserId = 42,
            OrderedProductIds = new List<int> { 1, 2 }
        };

        // Create a list of OrderedProductDto
        var products = new List<OrderedProductDto>
        {
            new OrderedProductDto { Id = 1, Name = "Product1", Quantity = 2, Price = 10.0m, OrderID = 1 },
            new OrderedProductDto { Id = 2, Name = "Product2", Quantity = 1, Price = 20.0m, OrderID = 1 }
        };

        // Pass them to the OrderViewModel constructor
        _orderViewModel = new OrderViewModel(order, products);
    }

    [Test]
    public void TestCalculateTotal()
    {
        decimal total = _orderViewModel.CalculateTotal();
        total.ShouldBe(40.0m);
    }

    [Test]
    public void TestOrderSummary()
    {
        string summary = _orderViewModel.OrderSummary;
        summary.ShouldBe("Order 04/21/2025 - Total: $40.0");
    }
}
