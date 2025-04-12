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
}
