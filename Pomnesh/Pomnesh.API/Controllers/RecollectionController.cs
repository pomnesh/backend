using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Recollection")]
[ApiController]
public class RecollectionController(RecollectionService service) : ControllerBase
{
    private readonly RecollectionService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateRecollection([FromBody] RecollectionCreateDto model)
    {
        int newId = await _service.Create(model);
        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetRecollection), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecollection(long id)
    {
        var result = await _service.Get(id);
        if (result == null)
            return NotFound(new { message = $"Recollection with ID {id} not found." });

        var response = new BaseApiResponse<Recollection> { Payload = result };
        return new JsonResult(response) { StatusCode = 200 };
    }
}