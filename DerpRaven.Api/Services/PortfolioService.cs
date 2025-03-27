using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class PortfolioService : IPortfolioService
{
    private AppDbContext _context;
    private ILogger _logger;

    public PortfolioService(AppDbContext context, ILogger<PortfolioService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<PortfolioDto>> GetAllPortfoliosAsync()
    {
        return await _context.Portfolios
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Select(p => MapToPortfolioDto(p))
            .ToListAsync();
    }

    public async Task<PortfolioDto?> GetPortfolioByIdAsync(int id)
    {
        return await _context.Portfolios
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Where(p => p.Id == id)
            .Select(p => MapToPortfolioDto(p))
            .FirstOrDefaultAsync();
    }

    public async Task<List<PortfolioDto>> GetPortfoliosByTypeAsync(string productType)
    {
        return await _context.Portfolios
            .Include(p => p.ProductType)
            .Include(p => p.Images)
            .Where(p => p.ProductType.Name == productType)
            .Select(p => MapToPortfolioDto(p))
            .ToListAsync();
    }


    public async Task<List<PortfolioDto>> GetPortfoliosByNameAsync(string name)
    {
        string searchQuery = name.Trim().ToLower();
        return await _context.Portfolios
            .Where(p => p.Name.Contains(searchQuery))
            .Select(p => MapToPortfolioDto(p))
            .ToListAsync();
    }

    public async Task<bool> CreatePortfolioAsync(PortfolioDto dto)
    {
        var portfolio = await MapFromPortfolioDto(dto);
        if (portfolio != null)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> UpdatePortfolioAsync(PortfolioDto dto)
    {
        var oldPortfolio = await _context.Portfolios.FindAsync(dto.Id);
        var productType = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        var images = await _context.Images
            .Where(i => dto.ImageIds.Contains(i.Id))
            .ToListAsync();

        if (oldPortfolio != null && productType != null)
        {
            oldPortfolio.Name = dto.Name;
            oldPortfolio.Description = dto.Description;
            oldPortfolio.ProductType = productType;
            oldPortfolio.Images = images;
            await _context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task DeletePortfolioAsync(int id)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);
        if (portfolio != null)
        {
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<Portfolio?> MapFromPortfolioDto(PortfolioDto dto)
    {
        var productType = await _context.ProductTypes.FindAsync(dto.ProductTypeId);
        var images = await _context.Images
            .Where(i => dto.ImageIds.Contains(i.Id))
            .ToListAsync();
        if (productType == null) return null;

        return new Portfolio()
        {
            Id = dto.Id,
            Description = dto.Description,
            Name = dto.Name,
            ProductType = productType,
            Images = images
        };
    }

    private static PortfolioDto MapToPortfolioDto(Portfolio portfolio)
    {
        List<int> imageIds = portfolio.Images
            .Select(p => p.Id)
            .ToList();

        return new PortfolioDto()
        {
            Id = portfolio.Id,
            Description = portfolio.Description,
            Name = portfolio.Name,
            ProductTypeId = portfolio.ProductType.Id,
            ImageIds = imageIds
        };
    }
}
