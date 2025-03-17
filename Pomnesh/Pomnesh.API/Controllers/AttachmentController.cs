using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;
using Swashbuckle.AspNetCore.Annotations;

namespace Pomnesh.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class AttachmentController(AttachmentsService service) : ControllerBase
{
    private readonly AttachmentsService _service = service;

    [HttpPost("Attachment")]
    public async Task<IActionResult> CreateAttachment(AttachmentCreateDto model)
    {
        int newId = await _service.Create(model);
        Console.WriteLine(model.OwnerId);
        
        var response = new BaseApiResponse<int>{Payload=newId};
        return new JsonResult(response) { StatusCode = 201 };
    }
    
    [HttpGet("Attachment/{id}")]
    public async Task<ActionResult<Attachment?>> GetAttachment(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Attachment with ID {id} not found." });
        
        var response = new BaseApiResponse<Attachment>{Payload = result};
        return new JsonResult(response) { StatusCode = 200 };
    }
}