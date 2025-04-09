using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomRequestController : ControllerBase
{
    private readonly ICustomRequestService _customRequestService;
    private readonly IDerpRavenMetrics _metrics;

    public CustomRequestController(ICustomRequestService customRequestService, IDerpRavenMetrics metrics)
    {
        _customRequestService = customRequestService;
        _metrics = metrics;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomRequests()
    {
        _metrics.AddCustomRequestEndpointCall();
        var requests = await _customRequestService.GetAllCustomRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomRequestById(int id)
    {
        _metrics.AddCustomRequestEndpointCall();
        var request = await _customRequestService.GetCustomRequestByIdAsync(id);
        if (request == null) return NoContent();
        return Ok(request);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetCustomRequestsByUserEmail()
    {
        _metrics.AddCustomRequestEndpointCall();
        var user = HttpContext.User;
        var userEmail = user?.Claims.FirstOrDefault(c => c.Type == "email")?.Value
            ?? user?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
        if (userEmail != null) userEmail = userEmail + "@snow.edu";

        if (userEmail == null) return Unauthorized();

        var requests = await _customRequestService.GetCustomRequestsByUserEmailAsync(userEmail);
        return Ok(requests);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetCustomRequestsByStatus(string status)
    {
        _metrics.AddCustomRequestEndpointCall();
        var requests = await _customRequestService.GetCustomRequestsByStatusAsync(status);
        return Ok(requests);
    }

    [HttpGet("type/{productType}")]
    public async Task<IActionResult> GetCustomRequestsByType(string productType)
    {
        _metrics.AddCustomRequestEndpointCall();
        var requests = await _customRequestService.GetCustomRequestsByTypeAsync(productType);
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomRequest([FromBody] CustomRequestDto request)
    {
        _metrics.AddCustomRequestEndpointCall();
        bool wasCreated = await _customRequestService.CreateCustomRequestAsync(request);
        if (!wasCreated) return BadRequest();
        return Created();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] string status)
    {
        _metrics.AddCustomRequestEndpointCall();
        bool wasUpdated = await _customRequestService.ChangeStatusAsync(id, status);
        if (!wasUpdated) return BadRequest();
        return NoContent();
    }
}