using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class UserService : BaseClass
{
    public UserService(AppDbContext context, ILogger<UserService> logger) : base(context, logger)
    {
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUser(User user)
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

    public async Task<IEnumerable<User>> GetUsersByStatus(bool active)
    {
        return await _context.Users.Where(u => u.Active == active).ToListAsync();
    }

    public async Task<IEnumerable<User>> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).ToListAsync();
    }

    public async Task<User?> GetUserByName(string name)
    {
        return await _context.Users.Where(u => u.Name == name).FirstOrDefaultAsync();
    }
}
