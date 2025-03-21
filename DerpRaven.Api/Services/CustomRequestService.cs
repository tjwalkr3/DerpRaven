using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class CustomRequestService
{
    private AppDbContext _context;
    private ILogger _logger;

    public CustomRequestService(AppDbContext context, ILogger<CustomRequestService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<CustomRequest>> GetAllCustomRequestsAsync()
    {
        return await _context.CustomRequests.ToListAsync();
    }

    public async Task<IEnumerable<CustomRequest>> GetCustomRequestsByUserAsync(int id)
    {
        return await _context.CustomRequests.Where(r => r.User.Id == id).ToListAsync();
    }

    public async Task<CustomRequest?> GetCustomRequestById(int id)
    {
        return await _context.CustomRequests.FindAsync(id);
    }

    public async Task<IEnumerable<CustomRequest>> GetCustomRequestsByStatusAsync(string status)
    {
        return await _context.CustomRequests.Where(r => r.Status == status).ToListAsync();
    }

    public async Task<IEnumerable<CustomRequest>> GetCustomRequestsByTypeAsync(string productType)
    {
        return await _context.CustomRequests.Where(r => r.ProductType.Name == productType).ToListAsync();
    }

    public async Task ChangeStatus(int id, string status)
    {
        var request = await _context.CustomRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateCustomRequest(CustomRequest request)
    {
        await _context.CustomRequests.AddAsync(request);
        await _context.SaveChangesAsync();
    }
}
