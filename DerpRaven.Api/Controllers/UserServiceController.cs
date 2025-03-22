using DerpRaven.Api.Model;
using DerpRaven.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DerpRaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserServiceController : ControllerBase
{
    private readonly UserService _userService;

    public UserServiceController(UserService userService)
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
    public async Task<IActionResult> GetUsersByEmail(string email)
    {
        var users = await _userService.GetUsersByEmailAsync(email);
        return Ok(users);
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
    public async Task<IActionResult> CreateUser(User user)
    {
        var createdUser = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }
}

