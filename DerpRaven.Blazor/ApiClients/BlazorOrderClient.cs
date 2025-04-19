using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;

namespace DerpRaven.Blazor.ApiClients;

public class BlazorOrderClient : IOrderClient
{
    private readonly HttpClient _httpClient;

    public BlazorOrderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        var response = await _httpClient.GetAsync("api/Order");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<OrderDto>>() ?? [];
    }

    public async Task CreateOrderAsync(OrderDto order)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<List<OrderDto>> GetOrdersByUserEmailAsync()
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    Task<int> IOrderClient.CreateOrderAsync(OrderDto order)
    {
        throw new NotImplementedException();
    }
}
