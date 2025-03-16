using Microsoft.AspNetCore.Mvc;
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
        await _service.Create(model);
        return Ok();
    }
    
    [HttpGet("Recollection/{id}")]
    public async Task<ActionResult<Recollection?>> GetRecollection(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Recollection with ID {id} not found." });

        return new JsonResult(new { data = result }) { StatusCode = 200 };
    }
}