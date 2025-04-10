using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DerpRaven.Shared.Dtos;
using System.Collections.ObjectModel;

namespace DerpRaven.Maui.ViewModels;

public partial class OrderHistoryPageViewModel : ObservableObject {
    public ObservableCollection<OrderViewModel> Orders { get; } = [];

    public OrderHistoryPageViewModel() {
        var historyList = new List<OrderDto>
        {
            new() { Id = 1, Address = "123 Street", Email = "user@example.com", OrderDate = DateTime.Now, UserId = 5, ProductIds = new List<int> { 1, 2 } },
            new() { Id = 2, Address = "456 Avenue", Email = "test@example.com", OrderDate = DateTime.Now.AddDays(-1), UserId = 6, ProductIds = new List<int> { 3, 4 } }
        };

        Orders.Clear();
        foreach (var order in historyList) {
            Orders.Add(new OrderViewModel(order));
        }
    }
}

public partial class OrderViewModel : ObservableObject {
    public OrderDto Order { get; }

    [ObservableProperty]
    private bool isExpanded;

    public OrderViewModel(OrderDto order) {
        Order = order;
    }

    [RelayCommand]
    private void ToggleExpand() {
        IsExpanded = !IsExpanded;
    }

    public DateTime OrderDate => Order.OrderDate;

    public List<string> productImagePaths = new();

    public void getImagePaths() {

        foreach(int imageId in Order.ProductIds) {
            string imagepath= ProductsListPageViewModel.ProductsList
            productImagePaths.Add();
        }
    }
}
