using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.Dtos;
namespace DerpRaven.Blazor.Pages;

public partial class Orders
{
    private List<OrderDto>? _orders = [];
    private string errorString = string.Empty;
    IBlazorOrderClient _orderClient { get; }

    public Orders(IBlazorOrderClient orderClient)
    {
        _orderClient = orderClient;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadOrders();
    }

    private async Task LoadOrders()
    {
        try
        {
            _orders = await _orderClient.GetAllOrdersAsync();
            errorString = string.Empty;
        }
        catch (Exception ex)
        {
            errorString = ex.Message;
        }
    }
}

