using DerpRaven.Api.Model;
using DerpRaven.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DerpRaven.Api.Services;

public class OrderedProductService : IOrderedProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public OrderedProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<OrderedProductDto>> GetOrderedProductsByOrderId(int orderId)
    {
        _logger.LogInformation("Fetching all ordered products");
        return await _context.OrderedProducts
            .Where(op => op.Order.Id == orderId)
            .Include(op => op.Order)
            .Select(op => MapToOrderedProductDto(op))
            .ToListAsync();
    }

    public async Task<bool> CreateOrderedProducts(List<OrderedProductDto> orderedProducts)
    {
        _logger.LogInformation("Creating ordered products");
        List<OrderedProduct> newOrderedProducts =
            (await Task.WhenAll(
                orderedProducts.Select(async orderedProduct => await MapToOrderedProduct(orderedProduct))
            )).ToList();

        if (newOrderedProducts.Count == 0)
        {
            _logger.LogWarning("No ordered products to create");
            return false;
        }
        _context.OrderedProducts.AddRange(newOrderedProducts);
        return true;
    }

    private static OrderedProductDto MapToOrderedProductDto(OrderedProduct orderedProduct)
    {
        return new OrderedProductDto
        {
            Id = orderedProduct.Id,
            Name = orderedProduct.Name,
            Quantity = orderedProduct.Quantity,
            Price = orderedProduct.Price
        };
    }

    private async Task<OrderedProduct> MapToOrderedProduct(OrderedProductDto orderedProductDto)
    {
        Order? newOrder = await _context.Orders.FindAsync(orderedProductDto.OrderID);
        if (newOrder == null) throw new Exception("Order not found");

        return new OrderedProduct
        {
            Id = orderedProductDto.Id,
            Name = orderedProductDto.Name,
            Quantity = orderedProductDto.Quantity,
            Price = orderedProductDto.Price,
            Order = newOrder
        };
    }
}
