using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class UserController(UsersService service) : ControllerBase
{
    private readonly UsersService _service = service;

    [HttpPost("User")]
    public async Task<IActionResult> CreateUser(UserCreateDto model)
    {
        await _service.Create(model);
        return Ok();
    }
    
    [HttpGet("User/{id}")]
    public async Task<ActionResult<User?>> GetUserInfo(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        return new JsonResult(new { data = result }) { StatusCode = 200 };
    }
}