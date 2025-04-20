using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;

public partial class OrderHistoryPageViewModel : ObservableObject
{
    private readonly IOrderClient _orderClient;
    private readonly IOrderedProductClient _orderedProductClient;
    public ObservableCollection<OrderViewModel> OrderViewModels { get; } = [];

    [ObservableProperty]
    private bool isLoading;
    public OrderHistoryPageViewModel(IOrderClient orderClient, IOrderedProductClient orderedProductClient)
    {
        _orderClient = orderClient;
        _orderedProductClient = orderedProductClient;

        var ghostHistoryList = new List<OrderDto>
        {
            new() { Id = 1, Address = "123 Street", Email = "user@example.com", OrderDate = DateTime.Now, UserId = 5, OrderedProductIds = new List<int> { 1, 2 } },
            new() { Id = 2, Address = "456 Avenue", Email = "test@example.com", OrderDate = DateTime.Now.AddDays(-1), UserId = 6, OrderedProductIds = new List<int> { 3, 4 } }
        };

        var ghostOrderedProductList = new List<OrderedProductDto>
        {
            new() { Id = 1, Name = "Product 1", Price = 10.99m, Quantity = 5, OrderID = 1},
            new() { Id = 2, Name = "Product 2", Price = 12.99m, Quantity = 10, OrderID = 1},
            new() { Id = 3, Name = "Product 3", Price = 15.99m, Quantity = 31, OrderID = 2},
            new() { Id = 4, Name = "Product 4", Price = 20.99m, Quantity = 18, OrderID = 2}
        };
    }

    public async Task RefreshOrdersView()
    {
        IsLoading = true;
        try
        {
            OrderViewModels.Clear();
            List<OrderDto> historyList = await GetOrdersAsync();
            foreach (var order in historyList)
            {
                var products = await GetOrderedProductsAsync(order.Id);
                OrderViewModels.Add(new OrderViewModel(order, products));
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderClient.GetOrdersByUserEmailAsync();
        orders.Reverse();
        return orders;
    }

    public async Task<List<OrderedProductDto>> GetOrderedProductsAsync(int orderId)
    {
        var products = await _orderedProductClient.GetOrderedProductsByOrderId(orderId);
        return products;
    }
}

public partial class OrderViewModel : ObservableObject
{
    public OrderDto Order { get; private set; }
    public decimal OrderTotal { get; private set; } = 0;

    public ObservableCollection<OrderedProductDto> Products { get; private set; } = [];

    [ObservableProperty]
    private bool isExpanded;

    public OrderViewModel(OrderDto order, List<OrderedProductDto> products)
    {
        foreach (var product in products) Products.Add(product);
        Order = order;
        CalculateTotal();
    }

    [RelayCommand]
    private void ToggleExpand()
    {
        IsExpanded = !IsExpanded;
    }

    public DateTime OrderDate => Order.OrderDate;
    public string OrderSummary => $"Order {OrderDate:MM/dd/yyyy} - Total: ${OrderTotal}";

    public decimal CalculateTotal()
    {
        return OrderTotal = Products.Sum(p => p.Quantity * p.Price);
    }
}
