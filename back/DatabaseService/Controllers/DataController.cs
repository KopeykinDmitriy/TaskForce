using Microsoft.AspNetCore.Mvc;
using DatabaseService.Services;
using DatabaseService.DTOs;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly UserService _userService;

    public DataController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("users")]
    public async Task<IActionResult> AddUser(UserDto userDto)
    {
        var user = await _userService.AddUserAsync(userDto);
        return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
    }
}