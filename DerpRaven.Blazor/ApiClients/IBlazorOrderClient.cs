using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorOrderClient
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
    }
}