using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/User")]
[ApiController]
public class UserController(IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

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

        var userResponse = new UserResponse
        {
            Id = result.Id,
            VkId = result.VkId,
            VkToken = result.VkToken,
        };

        var response = new BaseApiResponse<UserResponse> { Payload = userResponse };
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        
        List<UserResponse> userResponse = new List<UserResponse>();
        foreach (var user in result)
        {
            var responseDto = new UserResponse
            {
                Id = user.Id,
                VkId = user.VkId,
                VkToken = user.VkToken,
            };
            userResponse.Add(responseDto);
        }
        var response = new BaseApiResponse<IEnumerable<UserResponse>> { Payload = userResponse };
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]  UserUpdateDto model)
    {
        // Check if User exist
        var user = await _service.Get(model.Id);
        if (user == null)
            return NotFound(new { message = $"Attachment with ID {model.Id} not found." });

        await _service.Update(model);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        // Check if User exist
        var user = await _service.Get(id);
        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        await _service.Delete(id);
        return NoContent();
    }
}