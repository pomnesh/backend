using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/ChatContext")]
[ApiController]
public class ChatContextController(IChatContextService service) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateContext([FromBody] ChatContextCreateDto model)
    {
        int newId = await service.Create(model);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetContext), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContext(long id)
    {
        var result = await service.Get(id);

        var response = new BaseApiResponse<ChatContextResponse> { Payload = result };
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await service.GetAll();

        var response = new BaseApiResponse<IEnumerable<ChatContextResponse>> { Payload = result };
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateChatContext([FromBody]  ChatContextUpdateDto model)
    {
        await service.Update(model);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChatContext(long id)
    {
        await service.Delete(id);
        return NoContent();
    }
}