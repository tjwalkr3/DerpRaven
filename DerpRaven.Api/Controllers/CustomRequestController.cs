﻿using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
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
        UserDto? user = await ControllerHelpers.GetCurrentUser(HttpContext.User, _userService);
        if (user == null) return Unauthorized();

        var requests = await _customRequestService.GetCustomRequestsByUserEmailAsync(user.Email);
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomRequest([FromBody] CustomRequestDto request)
    {
        _metrics.AddCustomRequestEndpointCall();
        UserDto? user = await ControllerHelpers.GetCurrentUser(HttpContext.User, _userService);
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