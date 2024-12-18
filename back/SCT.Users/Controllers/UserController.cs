using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCT.Users.DTOs;
using SCT.Users.Services;

namespace SCT.Users.Controllers;

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

    [HttpGet("users-by-project")]
    public async Task<IActionResult> GetAllUserByProject(int projectId)
    {
        var users = await _userService.GetAllUsersByProjectAsync(projectId);
        return Ok(users);
    }

    [HttpPost("users")]
    public async Task<IActionResult> AddUser(UserDto userDto)
    {
        await _userService.AddUserAsync(userDto);
        return Ok();
    }

    [HttpPost("AddTagToUser")]
    public async Task<IActionResult> AddTagToUser([FromBody] List<TagDto> requests)
    {
        if (requests == null)
        {
            return BadRequest("Request cannot be null");
        }

        try
        {
            int userId = requests.First().UserId;
            List<string> tagNames = requests.Select(r => r.TagName).ToList();

            await _userService.AddTagsToUserAsync(userId, tagNames);

            return Ok("Tag successfully added to user.");
        }
        catch (Exception ex)
        {
            var innerException = ex.InnerException?.Message ?? "No inner exception.";
            return StatusCode(500, $"Error adding tag to user: {ex.Message}. Inner exception: {innerException}");
        }
    }

    [HttpGet("UserInfo")]
    public async Task<UserDto> GetUserInfoAsync()
    {
        return await _userService.GetUserInfo();
    }
}