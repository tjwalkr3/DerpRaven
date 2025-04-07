﻿using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class CustomRequestService : ICustomRequestService
{
    private AppDbContext _context;
    private ILogger _logger;

    public CustomRequestService(AppDbContext context, ILogger<CustomRequestService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<CustomRequestDto>> GetAllCustomRequestsAsync()
    {
        _logger.LogInformation("Fetching all custom requests");
        return await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Select(r => MapToCustomRequestDto(r))
            .ToListAsync();
    }

    public async Task<List<CustomRequestDto>> GetCustomRequestsByUserIdAsync(int id)
    {
        _logger.LogInformation("Fetching custom requests for user with ID {UserId}", id);
        return await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.User.Id == id)
            .Select(r => MapToCustomRequestDto(r))
            .ToListAsync();
    }

    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        _logger.LogInformation("Fetching custom request with ID {RequestId}", id);
        return await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.Id == id)
            .Select(r => MapToCustomRequestDto(r))
            .FirstOrDefaultAsync();
    }

    public async Task<List<CustomRequestDto>> GetCustomRequestsByStatusAsync(string status)
    {
        _logger.LogInformation("Fetching custom requests with status {Status}", status);
        return await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.Status == status)
            .Select(r => MapToCustomRequestDto(r))
            .ToListAsync();
    }

    public async Task<List<CustomRequestDto>> GetCustomRequestsByTypeAsync(string productType)
    {
        _logger.LogInformation("Fetching custom requests with product type {ProductType}", productType);
        return await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.ProductType.Name == productType)
            .Select(r => MapToCustomRequestDto(r))
            .ToListAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id, string status)
    {
        _logger.LogInformation("Changing status of custom request with ID {RequestId} to {Status}", id, status);
        var request = await _context.CustomRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CreateCustomRequestAsync(CustomRequestDto dto)
    {
        var customRequest = await MapFromCustomRequestDto(dto);
        if (customRequest != null)
        {
            await _context.CustomRequests.AddAsync(customRequest);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created custom request with ID {RequestId}", dto.Id);
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<CustomRequest?> MapFromCustomRequestDto(CustomRequestDto dto)
    {
        var productType = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        var user = await _context.Users.FindAsync(dto.UserId);
        if (productType == null || user == null) return null;

        return new CustomRequest()
        {
            Id = dto.Id,
            Description = dto.Description,
            Email = dto.Email,
            Status = dto.Status,
            ProductType = productType,
            User = user
        };
    }

    private static CustomRequestDto MapToCustomRequestDto(CustomRequest cr)
    {
        return new CustomRequestDto()
        {
            Id = cr.Id,
            Description = cr.Description,
            Email = cr.Email,
            Status = cr.Status,
            ProductTypeId = cr.ProductType.Id,
            UserId = cr.User.Id
        };
    }
}
