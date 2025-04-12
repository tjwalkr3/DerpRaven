using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients;

public class ProductClient(IApiService apiService) : IProductClient
{
    // does not need authentication
    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var response = await apiService.GetFromJsonAsyncWithoutAuthorization<List<ProductDto>>("api/product");
        return response ?? [];
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var response = await apiService.GetFromJsonAsyncWithoutAuthorization<ProductDto>($"api/product/{id}");
        return response;
    }
}