using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;

namespace DerpRaven.Blazor.ApiClients;

public class BlazorOrderClient : IBlazorOrderClient
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
}
