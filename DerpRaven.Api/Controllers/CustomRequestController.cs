using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Reflection.Metadata.Ecma335;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomRequestController : ControllerBase
{
    private readonly ICustomRequestService _customRequestService;
    private readonly IDerpRavenMetrics _metrics;
    private readonly IUserService _userService;

    public CustomRequestController(ICustomRequestService customRequestService, IDerpRavenMetrics metrics, IUserService userService)
    {
        _customRequestService = customRequestService;
        _metrics = metrics;
        _userService = userService;
    }

    public async Task<UserDto?> GetCurrentUser(ClaimsPrincipal user)
    {
        if (user == null) return null;
        string? userEmail = user?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (userEmail == null)
        {
            userEmail = user?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            if (userEmail != null) userEmail = userEmail + "@snow.edu";
        }

        if (userEmail == null) return null;
        return await _userService.GetUserByEmailAsync(userEmail);
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
        UserDto? user = await GetCurrentUser(HttpContext.User);
        if (user == null) return Unauthorized();

        var requests = await _customRequestService.GetCustomRequestsByUserEmailAsync(user.Email);
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
        UserDto? user = await GetCurrentUser(HttpContext.User);
        if (user == null) return Unauthorized();
        request.UserId = user.Id;

        return await _customRequestService.CreateCustomRequestAsync(request) ? Created() : BadRequest();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] string status)
    {
        _metrics.AddCustomRequestEndpointCall();
        return await _customRequestService.ChangeStatusAsync(id, status) ? NoContent() : BadRequest();
    }
}