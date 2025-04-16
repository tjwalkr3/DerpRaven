using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients
{
    public interface IOrderedProductClient
    {
        Task<bool> CreateOrderedProducts(List<OrderedProductDto> orderedProducts);
        Task<List<OrderedProductDto>> GetOrderedProductsByOrderId(int orderId);
    }
}