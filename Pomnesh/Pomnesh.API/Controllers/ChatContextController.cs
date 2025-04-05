using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.DTO;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/ChatContext")]
[ApiController]
public class ChatContextController(ChatContextService service) : ControllerBase
{
    private readonly ChatContextService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateContext([FromBody] ChatContextCreateDto model)
    {
        int newId = await _service.Create(model);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetContext), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContext(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Context with ID {id} not found." });
        
        var chatContextResponse = new ChatContextResponse
        {
            Id = result.Id,
            MessageId = result.MessageId,
            MessageText = result.MessageText
        };
        if (chatContextResponse == null) throw new ArgumentNullException(nameof(chatContextResponse));

        var response = new BaseApiResponse<ChatContextResponse> { Payload = chatContextResponse };
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        
        List<ChatContextResponse> chatContextResponse = new List<ChatContextResponse>();
        foreach (var chatContext in result)
        {
            var responseDto = new ChatContextResponse
            {
                Id = chatContext.Id,
                MessageId = chatContext.MessageId,
                MessageText = chatContext.MessageText,
                MessageDate = chatContext.MessageDate
            };
            chatContextResponse.Add(responseDto);
        }

        var response = new BaseApiResponse<IEnumerable<ChatContextResponse>> { Payload = chatContextResponse };
        return Ok(response);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateChatContext([FromBody]  ChatContextUpdateDto model)
    {
        // Check if ChatContext exist
        var chatContext = await _service.Get(model.Id);
        if (chatContext == null)
            return NotFound(new { message = $"Context with ID {model.Id} not found." });
        
        await _service.Update(model);
        return NoContent();
    }
}