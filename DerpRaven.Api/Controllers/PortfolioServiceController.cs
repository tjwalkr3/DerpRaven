using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioServiceController : ControllerBase
{
    private readonly PortfolioService _portfolioService;

    public PortfolioServiceController(PortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPortfolios()
    {
        var portfolios = await _portfolioService.GetAllPortfoliosAsync();
        return Ok(portfolios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPortfolioById(int id)
    {
        var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
        if (portfolio == null)
        {
            return NotFound();
        }
        return Ok(portfolio);
    }

    [HttpGet("type/{productType}")]
    public async Task<IActionResult> GetPortfoliosByType(string productType)
    {
        var portfolios = await _portfolioService.GetPortfoliosByTypeAsync(productType);
        return Ok(portfolios);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetPortfoliosByName(string name)
    {
        var portfolios = await _portfolioService.GetPortfoliosByNameAsync(name);
        return Ok(portfolios);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePortfolio(Portfolio portfolio)
    {
        var createdPortfolio = await _portfolioService.CreatePortfolioAsync(portfolio);
        return CreatedAtAction(nameof(GetPortfolioById), new { id = createdPortfolio.Id }, createdPortfolio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePortfolio(int id, Portfolio portfolio)
    {
        if (id != portfolio.Id)
        {
            return BadRequest();
        }

        await _portfolioService.UpdatePortfolioAsync(portfolio);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePortfolio(int id)
    {
        await _portfolioService.DeletePortfolioAsync(id);
        return NoContent();
    }
}
