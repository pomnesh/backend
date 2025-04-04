using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Attachment")]
[ApiController]
public class AttachmentController(AttachmentService service) : ControllerBase
{
    private readonly AttachmentService _service = service;

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
        
        var AttachmentResponse = new AttachmentResponseDto
        {
            Id = result.Id,
            Type = (AttachmentTypeDto)result.Type,
            FileId = result.FileId,
            OwnerId = result.OwnerId,
            OriginalLink = result.OriginalLink,
            ContextId = result.ContextId
            
        };
        var response = new BaseApiResponse<AttachmentResponseDto> { Payload = AttachmentResponse };
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttachmentResponseDto>>> GetAll()
    {
        var result = await _service.GetAll();
        
        List<AttachmentResponseDto> AttachmentResponse = new List<AttachmentResponseDto>();
        foreach (var attachment in result)
        {
            var responseDto = new AttachmentResponseDto
            {
                Id = attachment.Id,
                Type = (AttachmentTypeDto)attachment.Type,
                FileId = attachment.FileId,
                OwnerId = attachment.OwnerId,
                OriginalLink = attachment.OriginalLink,
                ContextId = attachment.ContextId
            
            };
            AttachmentResponse.Add(responseDto);
        }
        

        var response = new BaseApiResponse<IEnumerable<AttachmentResponseDto>> { Payload = AttachmentResponse };
        return Ok(response);
    }
}

