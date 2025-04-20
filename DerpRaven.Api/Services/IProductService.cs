using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductDto dto);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(ProductDto dto);
    }
}