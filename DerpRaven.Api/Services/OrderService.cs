using DerpRaven.Api.Dtos;
using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class OrderService : IOrderService
{
    private AppDbContext _context;
    private ILogger _logger;

    public OrderService(AppDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Products)
            .Select(o => MapToOrderDto(o))
            .ToListAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Products)
            .Where(o => o.Id == id)
            .Select(o => MapToOrderDto(o))
            .FirstOrDefaultAsync();
    }

    public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Products)
            .Where(r => r.User.Id == id)
            .Select(o => MapToOrderDto(o))
            .ToListAsync();
    }

    public async Task<bool> UpdateOrderAsync(int id, string address, string email)
    {
        var oldOrder = await _context.Orders.FindAsync(id);
        if (oldOrder != null)
        {
            oldOrder.Address = address;
            oldOrder.Email = email;
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CreateOrderAsync(OrderDto dto)
    {
        var order = await MapFromOrderDto(dto);
        if (order != null)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<Order?> MapFromOrderDto(OrderDto dto)
    {
        var products = await _context.Products
            .Where(p => dto.ProductIds.Contains(p.Id))
            .ToListAsync();
        var user = await _context.Users.FindAsync(dto.UserId);
        if (products == null || user == null) return null;

        return new Order()
        {
            Id = dto.Id,
            Address = dto.Address,
            Email = dto.Email,
            OrderDate = dto.OrderDate,
            User = user,
            Products = products
        };
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        List<int> productIds = order.Products
            .Select(o => o.Id)
            .ToList();

        return new OrderDto()
        {
            Id = order.Id,
            Address = order.Address,
            Email = order.Email,
            OrderDate = order.OrderDate,
            UserId = order.User.Id,
            ProductIds = productIds
        };
    }
}

