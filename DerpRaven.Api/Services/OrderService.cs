using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class OrderService
{
    private AppDbContext _context;
    private ILogger _logger;

    public OrderService(AppDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        var oldOrder = await _context.Orders.Where(o => o.Id == order.Id).FirstOrDefaultAsync();
        if (oldOrder != null)
        {
            oldOrder.Address = order.Address;
            oldOrder.Email = order.Email;
            _context.Update(oldOrder);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int id)
    {
        return await _context.Orders.Where(o => o.User.Id == id).ToListAsync();
    }

    public async Task<Order?> CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }
}

