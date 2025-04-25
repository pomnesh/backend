using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Models;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Attachment")]
[ApiController]
public class AttachmentController(IAttachmentService attachmentService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateAttachment([FromBody] AttachmentCreateRequest request)
    {
        int newId = await attachmentService.Create(request);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetAttachment), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttachment(long id)
    {
        var result = await attachmentService.Get(id);
        if (result == null)
            throw new AttachmentNotFoundError(id);
        
       
        var response = new BaseApiResponse<AttachmentResponse> { Payload = result };
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttachmentResponse>>> GetAll()
    {
        var result = await attachmentService.GetAll();

        var response = new BaseApiResponse<IEnumerable<AttachmentResponse>> { Payload = result };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAttachment([FromBody]  AttachmentUpdateDto model)
    {
        await attachmentService.Update(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttachment(long id)
    {
        await attachmentService.Delete(id);
        return NoContent();
    }
}

