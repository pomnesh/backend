using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;

namespace Pomnesh.API.Controllers;

[Route("api/v1/User")]
[ApiController]
public class UserController(IUserService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto model)
    {
        int newId = await service.Create(model);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetUserInfo), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(long id)
    {
        var result = await service.Get(id);
        
        var response = new BaseApiResponse<UserResponse> { Payload = result };
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAll();
        
        var response = new BaseApiResponse<IEnumerable<UserResponse>> { Payload = result };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]  UserUpdateDto model)
    {
        await service.Update(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {

        await service.Delete(id);
        return NoContent();
    }
}