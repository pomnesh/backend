using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AttachmentController(AttachmentsService service) : ControllerBase
{
    private readonly AttachmentsService _service = service;

    [HttpPost("attachment")]
    public async Task<IActionResult> CreateAttachment(AttachmentCreateDto model)
    {
        await _service.Create(model);
        Console.WriteLine(model.OwnerId);
        return Ok();
    }
}