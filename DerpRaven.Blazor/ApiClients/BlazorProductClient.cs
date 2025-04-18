using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;

namespace DerpRaven.Blazor.ApiClients;

public class BlazorProductClient
{
    private readonly HttpClient _httpClient;

    public BlazorProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreateProductAsync(ProductDto order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Product", order);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/product");
        return response ?? [];
    }
}
