using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Attachment")]
[ApiController]
public class AttachmentController(AttachmentService attachmentService, ChatContextService chatContextService) : ControllerBase
{
    private readonly AttachmentService _attachmentService = attachmentService;
    private readonly ChatContextService _chatContextService = chatContextService;

    [HttpPost]
    public async Task<IActionResult> CreateAttachment([FromBody] AttachmentCreateDto model)
    {
        // Check if ChatContext exist.
        var chatContext = await _chatContextService.Get(model.ContextId);
        if (chatContext == null)
            return NotFound(new { message = $"ChatContext with ID {model.ContextId} not found." });
        
        int newId = await _attachmentService.Create(model);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetAttachment), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttachment(long id)
    {
        var result = await _attachmentService.Get(id);
        if (result == null)
            return NotFound(new { message = $"Attachment with ID {id} not found." });
        
        var attachmentResponse = new AttachmentResponseDto
        {
            Id = result.Id,
            Type = (AttachmentTypeDto)result.Type,
            FileId = result.FileId,
            OwnerId = result.OwnerId,
            OriginalLink = result.OriginalLink,
            ContextId = result.ContextId
        };
        var response = new BaseApiResponse<AttachmentResponseDto> { Payload = attachmentResponse };
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttachmentResponseDto>>> GetAll()
    {
        var result = await _attachmentService.GetAll();
        
        List<AttachmentResponseDto> attachmentResponse = new List<AttachmentResponseDto>();
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
            attachmentResponse.Add(responseDto);
        }
        

        var response = new BaseApiResponse<IEnumerable<AttachmentResponseDto>> { Payload = attachmentResponse };
        return Ok(response);
    }
}

