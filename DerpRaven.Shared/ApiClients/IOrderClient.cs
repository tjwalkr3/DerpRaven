using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients
{
    public interface IOrderClient
    {
        Task CreateOrderAsync(OrderDto order);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersByUserEmailAsync();
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
    }
}