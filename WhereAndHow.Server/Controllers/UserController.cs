using Application.Interfaces;
using Core.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WhereAndHow.Server.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController (IUserService userService): ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO dto)
    {
        var response = await _userService.UpdateUserAsync(id, dto);
        return response == null
            ? NotFound("User not found")
            : Ok(response);
    }

}
