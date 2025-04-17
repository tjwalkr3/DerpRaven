using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(OrderDto dto);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersByUserEmailAsync(string email);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int id);
        Task<bool> UpdateOrderAsync(int id, string address, string email);
    }
}