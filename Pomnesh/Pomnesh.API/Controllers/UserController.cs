using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Pomnesh.API.Controllers;

[Route("api/v1/User")]
[ApiController]
[Authorize]
public class UserController(IUserService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
    {
        int newId = await service.Create(request);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetUserInfo), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(long id)
    {
        var result = await service.Get(id);
        if (result == null)
            throw new UserNotFoundError(id);
        
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
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
    {
        var currentUser = await service.Get(request.Id);
        if (currentUser == null)
            throw new UserNotFoundError(request.Id);

        var updateRequest = new UserUpdateRequest
        {
            Id = request.Id,
            Username = request.Username ?? currentUser.Username,
            Email = request.Email ?? currentUser.Email,
            VkToken = request.VkToken ?? currentUser.VkToken
        };

        await service.Update(updateRequest);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {

        await service.Delete(id);
        return NoContent();
    }
}