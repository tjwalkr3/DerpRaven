using System.Net.Http.Json;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients;


public class OrderedProductClient(IApiService apiService) : IOrderedProductClient
{
    public async Task<List<OrderedProductDto>> GetOrderedProductsByOrderId(int orderId)
    {
        var response = await apiService.GetAsync($"api/OrderedProduct/{orderId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<OrderedProductDto>>() ?? [];
    }

    public async Task<bool> CreateOrderedProducts(List<OrderedProductDto> orderedProducts)
    {
        var response = await apiService.PostAsJsonAsync("api/OrderedProduct", orderedProducts);
        return response.IsSuccessStatusCode;
    }
}