using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/ChatContext")]
[ApiController]
public class ChatContextController(ChatContextsService service) : ControllerBase
{
    private readonly ChatContextsService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateContext([FromBody] ChatContextCreateDto model)
    {
        int newId = await _service.Create(model);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetContext), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ChatContext?>> GetContext(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Context with ID {id} not found." });

        var response = new BaseApiResponse<ChatContext> { Payload = result };
        return new JsonResult(response) { StatusCode = 200 };
    }
}