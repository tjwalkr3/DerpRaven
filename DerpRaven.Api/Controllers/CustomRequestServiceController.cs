using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using DerpRaven.Api.Dtos;
using System.Net;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomRequestController : ControllerBase
{
    private readonly CustomRequestService _customRequestService;

    public CustomRequestController(CustomRequestService customRequestService)
    {
        _customRequestService = customRequestService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomRequests()
    {
        var requests = await _customRequestService.GetAllCustomRequestsAsync();
        return Ok(requests);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomRequestById(int id)
    {
        var request = await _customRequestService.GetCustomRequestByIdAsync(id);
        if (request == null)
        {
            return NotFound();
        }
        return Ok(request);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetCustomRequestsByUser(int userId)
    {
        var requests = await _customRequestService.GetCustomRequestsByUserIdAsync(userId);
        return Ok(requests);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetCustomRequestsByStatus(string status)
    {
        var requests = await _customRequestService.GetCustomRequestsByStatusAsync(status);
        return Ok(requests);
    }

    [HttpGet("type/{productType}")]
    public async Task<IActionResult> GetCustomRequestsByType(string productType)
    {
        var requests = await _customRequestService.GetCustomRequestsByTypeAsync(productType);
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomRequest([FromBody] CustomRequestDto request)
    {
        await _customRequestService.CreateCustomRequestAsync(request);
        return Created();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] string status)
    {
        await _customRequestService.ChangeStatusAsync(id, status);
        return NoContent();
    }

}

