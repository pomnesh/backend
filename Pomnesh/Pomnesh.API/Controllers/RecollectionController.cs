using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Recollection")]
[ApiController]
public class RecollectionController(IRecollectionService recollectionService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateRecollection([FromBody] RecollectionCreateRequest request)
    {
        int newId = await recollectionService.Create(request);

        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetRecollection), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecollection(long id)
    {
        var result = await recollectionService.Get(id);
        
        var response = new BaseApiResponse<RecollectionResponse> { Payload = result };
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await recollectionService.GetAll();
        
        var response = new BaseApiResponse<IEnumerable<RecollectionResponse>> { Payload = result };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRecollection([FromBody]  RecollectionUpdateRequest request)
    {
        await recollectionService.Update(request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecollection(long id)
    {
        await recollectionService.Delete(id);
        return NoContent();
    }
}