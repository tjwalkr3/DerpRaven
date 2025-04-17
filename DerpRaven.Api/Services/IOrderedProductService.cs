using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IOrderedProductService
    {
        Task<bool> CreateOrderedProducts(List<OrderedProductDto> orderedProducts);
        Task<List<OrderedProductDto>> GetOrderedProductsByOrderId(int orderId);
    }
}