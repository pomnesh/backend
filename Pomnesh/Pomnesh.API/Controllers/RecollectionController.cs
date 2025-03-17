using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class RecollectionController(RecollectionsService service) : ControllerBase
{
    private readonly RecollectionsService _service = service;

    [HttpPost("Recollection")]
    public async Task<IActionResult> CreateRecollection(RecollectionCreateDto model)
    {
        int newId = await _service.Create(model);
        var response = new BaseApiResponse<int>{Payload=newId};
        return new JsonResult(response) { StatusCode = 201 };
    }
    
    [HttpGet("Recollection/{id}")]
    public async Task<ActionResult<Recollection?>> GetRecollection(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Recollection with ID {id} not found." });

        var response = new BaseApiResponse<Recollection>{Payload = result};
        return new JsonResult(response) { StatusCode = 200 };
    }
}