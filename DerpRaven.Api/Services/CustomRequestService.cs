using DerpRaven.Api.Dtos;
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

    public async Task<List<CustomRequestDto>> GetAllCustomRequestsAsync()
    {
        List<CustomRequestDto> dtos = [];
        var customRequests = await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .ToListAsync();

        customRequests.ForEach(cr => dtos.Add(MapToCustomRequestDto(cr)));
        return dtos;
    }

    public async Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByUserAsync(int id)
    {
        List<CustomRequestDto> dtos = [];
        var customRequests = await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.User.Id == id)
            .ToListAsync();

        customRequests.ForEach(cr => dtos.Add(MapToCustomRequestDto(cr)));
        return dtos;
    }

    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        var customRequest = await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.Id == id)
            .FirstAsync();

        if (customRequest == null) return null;
        return MapToCustomRequestDto(customRequest);
    }

    public async Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByStatusAsync(string status)
    {
        List<CustomRequestDto> dtos = [];
        var customRequests = await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.Status == status)
            .ToListAsync();

        customRequests.ForEach(cr => dtos.Add(MapToCustomRequestDto(cr)));
        return dtos;
    }

    public async Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByTypeAsync(string productType)
    {
        List<CustomRequestDto> dtos = [];
        var customRequests = await _context.CustomRequests
            .Include(r => r.ProductType)
            .Include(r => r.User)
            .Where(r => r.ProductType.Name == productType)
            .ToListAsync();

        customRequests.ForEach(cr => dtos.Add(MapToCustomRequestDto(cr)));
        return dtos;
    }

    public async Task ChangeStatusAsync(int id, string status)
    {
        var request = await _context.CustomRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = status;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateCustomRequestAsync(CustomRequestDto dto)
    {
        var customRequest = MapFromCustomRequestDto(dto);
        await _context.CustomRequests.AddAsync(customRequest);
        await _context.SaveChangesAsync();
    }

    private CustomRequest MapFromCustomRequestDto(CustomRequestDto dto)
    {
        var productType = _context.ProductTypes.Where(t => t.Id == dto.ProductTypeId).Single();
        var user = _context.Users.Where(u => u.Id == dto.UserId).Single();

        return new CustomRequest()
        {
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
            Description = cr.Description,
            Email = cr.Email,
            Status = cr.Status,
            ProductTypeId = cr.ProductType.Id,
            UserId = cr.User.Id
        };
    }
}
