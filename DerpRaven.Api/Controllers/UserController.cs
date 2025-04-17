using DerpRaven.Shared.Dtos;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("status/{active}")]
    public async Task<IActionResult> GetUsersByStatus(bool active)
    {
        var users = await _userService.GetUsersByStatusAsync(active);
        return Ok(users);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetUsersByName(string name)
    {
        var user = await _userService.GetUsersByNameAsync(name);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDto user)
    {
        var wasUser = await _userService.CreateUserAsync(user);
        if (!wasUser) return BadRequest();
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto user)
    {
        if (id != user.Id) return BadRequest();
        bool wasUpdated = await _userService.UpdateUserAsync(user);
        if (!wasUpdated) return NotFound();
        return NoContent();
    }
}

