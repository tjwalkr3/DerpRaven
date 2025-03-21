using DerpRaven.Api.Model;
using Microsoft.EntityFrameworkCore;
namespace DerpRaven.Api.Services;

public class PortfolioService : BaseClass
{
    public PortfolioService(AppDbContext context, ILogger<PortfolioService> logger) : base(context, logger)
    {
    }

    public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
    {
        return await _context.Portfolios.ToListAsync();
    }

    public async Task<Portfolio?> GetPortfolioById(int id)
    {
        return await _context.Portfolios.FindAsync(id);
    }

    public async Task<IEnumerable<Portfolio>> GetPortfoliosByTypeAsync(string type)
    {
        return await _context.Portfolios.Where(p => p.Type.Name == type).ToListAsync();
    }

    public async Task<Portfolio?> CreatePortfolio(Portfolio portfolio)
    {
        await _context.Portfolios.AddAsync(portfolio);
        await _context.SaveChangesAsync();
        return portfolio;
    }

    public async Task UpdatePortfolio(Portfolio portfolio)
    {
        var oldPortfolio = await _context.Portfolios.Where(p => p.Id == portfolio.Id).FirstOrDefaultAsync();
        if (oldPortfolio != null)
        {
            oldPortfolio.Name = portfolio.Name;
            oldPortfolio.Description = portfolio.Description;
            oldPortfolio.Type = portfolio.Type;
            oldPortfolio.Images = portfolio.Images;
            _context.Update(oldPortfolio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeletePortfolio(int id)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);
        if (portfolio != null)
        {
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Portfolio>> GetPortfoliosByNameAsync(string name)
    {
        return await _context.Portfolios.Where(p => p.Name.Contains(name)).ToListAsync();
    }
}   
