using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class UserService
{
    private AppDbContext _context;
    private ILogger _logger;

    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        var oldUser = await _context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
        if (oldUser != null)
        {
            oldUser.Name = user.Name;
            oldUser.Email = user.Email;
            oldUser.Active = user.Active;

            _context.Update(oldUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> GetUsersByStatusAsync(bool active)
    {
        return await _context.Users.Where(u => u.Active == active).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByEmailAsync(string email)
    {
        return await _context.Users.Where(u => u.Email == email).ToListAsync();
    }

    public async Task<User?> GetUserByNameAsync(string name)
    {
        return await _context.Users.Where(u => u.Name == name).FirstOrDefaultAsync();
    }
}
