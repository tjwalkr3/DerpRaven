using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;
namespace DerpRaven.Shared.ApiClients;

public class OrderClient(IApiService apiService) : IOrderClient
{
    public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
    {
        var response = await apiService.GetAsync($"api/Order/user/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<OrderDto>>() ?? [];
    }

    public async Task CreateOrderAsync(OrderDto order)
    {
        var response = await apiService.PostAsJsonAsync("api/Order", order);
        response.EnsureSuccessStatusCode();
    }
}