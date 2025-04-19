using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients
{
    public interface IProductClient
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(ProductDto product);
    }
}