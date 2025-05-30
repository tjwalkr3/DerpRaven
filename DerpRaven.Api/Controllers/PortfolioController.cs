﻿using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DerpRaven.Api.Model;
using Microsoft.AspNetCore.Authorization;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;
    private readonly IDerpRavenMetrics _metrics;

    public PortfolioController(IPortfolioService portfolioService, IDerpRavenMetrics metrics)
    {
        _portfolioService = portfolioService;
        _metrics = metrics;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPortfolios()
    {
        _metrics.AddPortfolioEndpointCall();
        var portfolios = await _portfolioService.GetAllPortfoliosAsync();
        return Ok(portfolios);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPortfolioById(int id)
    {
        _metrics.AddPortfolioEndpointCall();
        var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
        if (portfolio == null)
        {
            return NotFound();
        }
        return Ok(portfolio);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePortfolioAsync(PortfolioDto portfolio)
    {
        _metrics.AddPortfolioEndpointCall();
        var wasCreated = await _portfolioService.CreatePortfolioAsync(portfolio);
        if (!wasCreated) return BadRequest();
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePortfolio(PortfolioDto portfolio)
    {
        _metrics.AddPortfolioEndpointCall();
        bool wasUpdated = await _portfolioService.UpdatePortfolioAsync(portfolio);
        if (!wasUpdated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePortfolioAsync(int id)
    {
        _metrics.AddPortfolioEndpointCall();
        bool wasDeleted = await _portfolioService.DeletePortfolioAsync(id);
        if (!wasDeleted) return NotFound();
        return NoContent();
    }
}
