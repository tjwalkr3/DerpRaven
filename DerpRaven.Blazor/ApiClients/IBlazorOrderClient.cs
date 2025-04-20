using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorOrderClient
    {
        Task CreateOrderAsync(OrderDto order);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserEmailAsync();
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
    }
}