using DerpRaven.Api.Model;
using DerpRaven.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DerpRaven.Api.Services;

public class OrderedProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public OrderedProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<OrderedProductDto>> GetOrderedProductsByOrderId()
    {
        _logger.LogInformation("Fetching all ordered products");
        return await _context.OrderedProducts
            .Include(op => op.Order)
            .Select(op => MapToOrderedProductDto(op))
            .ToListAsync();
    }

    public async Task CreateOrderedProducts(List<OrderedProductDto> orderedProducts)
    {
        _logger.LogInformation("Creating ordered products");
        List<OrderedProduct> newOrderedProducts = 
            (await Task.WhenAll(
                orderedProducts.Select(async orderedProduct => await MapToOrderedProduct(orderedProduct))
            )).ToList();

        if (newOrderedProducts.Count == 0)
        {
            _logger.LogWarning("No ordered products to create");
            return;
        }
        _context.OrderedProducts.AddRange(newOrderedProducts);
    }

    public OrderedProductDto MapToOrderedProductDto(OrderedProduct orderedProduct)
    {
        return new OrderedProductDto
        {
            Id = orderedProduct.Id,
            Quantity = orderedProduct.Quantity,
            Price = orderedProduct.Price           
        };
    }

    public async Task<OrderedProduct> MapToOrderedProduct(OrderedProductDto orderedProductDto)
    {
        Order? newOrder = await _context.Orders.FindAsync(orderedProductDto.OrderID);
        if (newOrder == null) throw new Exception("Order not found");

        return new OrderedProduct
        {
            Id = orderedProductDto.Id,
            Quantity = orderedProductDto.Quantity,
            Price = orderedProductDto.Price,
            Order = newOrder
        };
    }
}
