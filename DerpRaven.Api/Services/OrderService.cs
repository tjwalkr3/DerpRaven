using DerpRaven.Shared.Dtos;
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
        _logger.LogInformation("Fetching all orders");
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderedProducts)
            .Select(o => MapToOrderDto(o))
            .ToListAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        _logger.LogInformation("Fetching order with ID {OrderId}", id);
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderedProducts)
            .Where(o => o.Id == id)
            .Select(o => MapToOrderDto(o))
            .FirstOrDefaultAsync();
    }

    public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int id)
    {
        _logger.LogInformation("Fetching orders for user with ID {UserId}", id);
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderedProducts)
            .Where(r => r.User.Id == id)
            .Select(o => MapToOrderDto(o))
            .ToListAsync();
    }

    // Test this later
    public async Task<List<OrderDto>> GetOrdersByUserEmailAsync(string email)
    {
        _logger.LogInformation("Fetching orders for user with ID {UserEmail}", email);
        //return await _context.Orders
        //    .Include(r => r.User)
        //    .Where(r => r.User.Email == email)
        //    .Select(r => MapToOrderDto(r))
        //    .ToListAsync();
        List<Order> orders = await _context.Orders.Include(r => r.User).ToListAsync();
        List<OrderDto> orderDtos = [];

        foreach (var order in orders)
        {
            if (order.User.Email == email)
            {
                OrderDto orderDto = MapToOrderDto(order);
                orderDtos.Add(orderDto);
            }
        }

        return orderDtos;
    }

    public async Task<bool> UpdateOrderAsync(int id, string address, string email)
    {
        var oldOrder = await _context.Orders.FindAsync(id);
        if (oldOrder != null)
        {
            oldOrder.Address = address;
            oldOrder.Email = email;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated order with ID {OrderId}", id);
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
            _logger.LogInformation("Created order with ID {OrderId}", dto.Id);
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<Order?> MapFromOrderDto(OrderDto dto)
    {
        var products = await _context.OrderedProducts
            .Where(p => dto.OrderedProductIds.Contains(p.Id))
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
            OrderedProducts = products
        };
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        List<int> orderedProductIds = order.OrderedProducts
            .Select(o => o.Id)
            .ToList();

        return new OrderDto()
        {
            Id = order.Id,
            Address = order.Address,
            Email = order.Email,
            OrderDate = order.OrderDate,
            UserId = order.User.Id,
            OrderedProductIds = orderedProductIds
        };
    }
}

