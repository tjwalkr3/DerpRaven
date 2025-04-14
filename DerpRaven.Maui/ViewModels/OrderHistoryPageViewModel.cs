using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;

public partial class OrderHistoryPageViewModel : ObservableObject
{
    public ObservableCollection<OrderViewModel> Orders { get; } = [];

    public OrderHistoryPageViewModel()
    {
        var ghostHistoryList = new List<OrderDto>
        {
            new() { Id = 1, Address = "123 Street", Email = "user@example.com", OrderDate = DateTime.Now, UserId = 5, ProductIds = new List<int> { 1, 2 } },
            new() { Id = 2, Address = "456 Avenue", Email = "test@example.com", OrderDate = DateTime.Now.AddDays(-1), UserId = 6, ProductIds = new List<int> { 3, 4 } }
        };

        var ghostProductList = new List<ProductDto>
        { 
            new() { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.99m, ImageIds = [1] },
            new() { Id = 2, Name = "Product 2", Description = "Description 2", Price = 12.99m, ImageIds = [1] },
            new() { Id = 3, Name = "Product 3", Description = "Description 3", Price = 15.99m, ImageIds = [1] },
            new() { Id = 4, Name = "Product 4", Description = "Description 4", Price = 20.99m, ImageIds = [1] }
        };

        UpdateOrderDtos(ghostHistoryList, ghostProductList);
    }

    public void UpdateOrderDtos(List<OrderDto> historyList, List<ProductDto> products)
    {
        Orders.Clear();
        foreach (var order in historyList)
        {
            Orders.Add(new OrderViewModel(order, products));
        }
    }
}

public partial class OrderViewModel : ObservableObject
{
    public OrderDto Order { get; private set; }
    public List<ProductDto> Products { get; private set; } = new();
    public decimal OrderTotal { get; private set; } = 0;

    [ObservableProperty]
    private bool isExpanded;

    public OrderViewModel(OrderDto order, List<ProductDto> productList)
    {
        Order = order;
        Products = productList.Where(p => order.ProductIds.Contains(p.Id)).ToList();
    }

    [RelayCommand]
    private void ToggleExpand()
    {
        IsExpanded = !IsExpanded;
    }

    public DateTime OrderDate => Order.OrderDate;
}
