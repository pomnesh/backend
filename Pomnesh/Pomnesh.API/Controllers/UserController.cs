using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
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
        int newId = await _service.Create(model);
        var response = new BaseApiResponse<int>{Payload=newId};
        return new JsonResult(response) { StatusCode = 201 };
        return Ok();
    }
    
    [HttpGet("User/{id}")]
    public async Task<ActionResult<User?>> GetUserInfo(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"User with ID {id} not found." });
        
        var response = new BaseApiResponse<User>{Payload = result};
        return new JsonResult(response) { StatusCode = 200 };
    }
}