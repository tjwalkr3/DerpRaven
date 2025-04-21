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

    public async Task<int> CreateOrderAsync(OrderDto order)
    {
        var response = await apiService.PostAsJsonAsync("api/Order", order);
        response.EnsureSuccessStatusCode();

        // Deserialize the response body to extract the createdOrderId
        var result = await response.Content.ReadFromJsonAsync<Response>();
        return result?.OrderId ?? 0;
    }

    public async Task<List<OrderDto>> GetOrdersByUserEmailAsync()
    {
        var response = await apiService.GetAsync($"api/Order/user");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<OrderDto>>() ?? [];
    }
}

public class Response()
{
    public int OrderId { get; set; }
}