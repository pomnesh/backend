using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Attachment")]
[ApiController]
public class AttachmentController(AttachmentsService service) : ControllerBase
{
    private readonly AttachmentsService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateAttachment([FromBody] AttachmentCreateDto model)
    {
        int newId = await _service.Create(model);
        Console.WriteLine(model.OwnerId);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetAttachment), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttachment(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Attachment with ID {id} not found." });

        var response = new BaseApiResponse<Attachment> { Payload = result };
        return Ok(response);
    }
}

