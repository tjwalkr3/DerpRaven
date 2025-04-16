using DerpRaven.Api.Model;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IOrderedProductService
    {
        Task CreateOrderedProducts(List<OrderedProductDto> orderedProducts);
        Task<List<OrderedProductDto>> GetOrderedProductsByOrderId();
        Task<OrderedProduct> MapToOrderedProduct(OrderedProductDto orderedProductDto);
        OrderedProductDto MapToOrderedProductDto(OrderedProduct orderedProduct);
    }
}