using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorProductClient
    {
        Task<bool> CreateProductAsync(ProductDto order);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<bool> UpdateProductAsync(ProductDto product);
    }
}