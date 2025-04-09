using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
namespace DerpRaven.Api.Services;

public class UserService : IUserService
{
    private AppDbContext _context;
    private ILogger _logger;

    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        _logger.LogInformation("Checking whether email exists: {UserEmail}", email);
        return await _context.Users
            .Where(u => u.Email == email)
            .AnyAsync();
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Fetching all users");
        return await _context.Users
            .Select(u => MapToUserDto(u))
            .ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Where(u => u.Id == id)
            .Select(u => MapToUserDto(u))
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserDto>> GetUsersByStatusAsync(bool active)
    {
        return await _context.Users
            .Where(u => u.Active == active)
            .Select(u => MapToUserDto(u))
            .ToListAsync();
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .Select(u => MapToUserDto(u))
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserDto>> GetUsersByNameAsync(string name)
    {
        string searchQuery = name.Trim().ToLower();
        return await _context.Users
            .Where(u => u.Name.Trim().ToLower() == searchQuery)
            .Select(u => MapToUserDto(u))
            .ToListAsync();
    }

    public async Task<bool> CreateUserAsync(UserDto dto)
    {
        var user = MapFromUserDto(dto);
        if (user != null)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(UserDto dto)
    {
        var oldUser = await _context.Users.FindAsync(dto.Id);
        if (oldUser != null)
        {
            oldUser.Name = dto.Name;
            oldUser.Email = dto.Email;
            oldUser.Active = dto.Active;
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static User MapFromUserDto(UserDto dto)
    {
        return new User()
        {
            Id = dto.Id,
            Name = dto.Name,
            OAuth = dto.OAuth,
            Email = dto.Email,
            Role = dto.Role,
            Active = dto.Active,
            CustomRequests = [],
            Orders = []
        };
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            OAuth = user.OAuth,
            Email = user.Email,
            Role = user.Role,
            Active = user.Active
        };
    }
}
