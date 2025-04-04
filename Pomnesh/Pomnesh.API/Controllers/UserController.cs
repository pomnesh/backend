using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

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

        var userResponse = new UserResponseDto
        {
            Id = result.Id,
            VkId = result.VkId,
            VkToken = result.VkToken,
        };

        var response = new BaseApiResponse<UserResponseDto> { Payload = userResponse };
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        
        List<UserResponseDto> userResponse = new List<UserResponseDto>();
        foreach (var user in result)
        {
            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                VkId = user.VkId,
                VkToken = user.VkToken,
            };
            userResponse.Add(responseDto);
        }
        var response = new BaseApiResponse<IEnumerable<UserResponseDto>> { Payload = userResponse };
        return Ok(response);
    }
}