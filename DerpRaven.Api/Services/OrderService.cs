﻿using DerpRaven.Shared.Dtos;
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
        return await _context.Orders
            .Include(r => r.User)
            .Where(r => r.User.Email == email)
            .Select(r => MapToOrderDto(r))
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
            _logger.LogInformation("Updated order with ID {OrderId}", id);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<int> CreateOrderAsync(OrderDto dto)
    {
        var order = await MapFromOrderDto(dto);
        if (order != null)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created order with ID {OrderId}", order.Id);
            return order.Id;
        }
        else
        {
            return 0;
        }
    }

    private async Task<Order?> MapFromOrderDto(OrderDto dto)
    {
        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null) return null;

        return new Order()
        {
            Address = dto.Address,
            Email = dto.Email,
            OrderDate = dto.OrderDate.ToUniversalTime(),
            User = user,
            OrderedProducts = []
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

