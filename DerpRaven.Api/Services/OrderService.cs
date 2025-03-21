using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class OrderService : BaseClass
{
    public OrderService(AppDbContext context, ILogger<OrderService> logger) : base(context, logger)
    {
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task UpdateOrder(Order order)
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

    public async Task<Order?> GetOrderByUserId(int id)
    {
        return await _context.Orders.Where(o => o.User.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Order?> CreateOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }
}

