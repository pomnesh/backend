using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/User")]
[ApiController]
public class UserController(UserService service) : ControllerBase
{
    private readonly UserService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto model)
    {
        int newId = await _service.Create(model);
        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetUserInfo), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        var response = new BaseApiResponse<User> { Payload = result };
        return new JsonResult(response) { StatusCode = 200 };
    }
}