using DerpRaven.Api.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(ProductDto dto);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetProductsByNameAsync(string name);
        Task<List<ProductDto>> GetProductsByTypeAsync(string productType);
        Task<bool> UpdateProductAsync(ProductDto dto);
    }
}