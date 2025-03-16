using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class AttachmentController(AttachmentsService service) : ControllerBase
{
    private readonly AttachmentsService _service = service;

    [HttpPost("Attachment")]
    public async Task<IActionResult> CreateAttachment(AttachmentCreateDto model)
    {
        await _service.Create(model);
        Console.WriteLine(model.OwnerId);
        return Ok();
    }
    
    [HttpGet("Attachment/{id}")]
    public async Task<ActionResult<Attachment?>> GetAttachment(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Attachment with ID {id} not found." });
        
        return new JsonResult(new { data = result }) { StatusCode = 200 };
        
    }
}