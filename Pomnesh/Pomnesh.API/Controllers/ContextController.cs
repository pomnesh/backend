using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/")]
[ApiController]
public class ContextController(ContextsService service) : ControllerBase
{
    private readonly ContextsService _service = service;

    [HttpPost("Context")]
    public async Task<IActionResult> CreateContext(ContextCreateDto model)
    {
        await _service.Create(model);
        return Ok();
    }
    
    [HttpGet("Context/{id}")]
    public async Task<ActionResult<Context?>> GetContext(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Context with ID {id} not found." });

        return new JsonResult(new { data = result }) { StatusCode = 200 };
    }
}