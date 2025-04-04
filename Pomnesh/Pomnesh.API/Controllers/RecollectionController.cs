using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

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

        var recollectionResponse = new RecollectionResponseDto
        {
            Id = result.Id,
            UserId = result.UserId,
            CreatedAt = result.CreatedAt,
            DownloadLink = result.DownloadLink
        };
        
        var response = new BaseApiResponse<RecollectionResponseDto> { Payload = recollectionResponse };
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAll();
        List<RecollectionResponseDto> recollectionResponse = new List<RecollectionResponseDto>();
        foreach (var recollection in result)
        {
            var responseDto = new RecollectionResponseDto
            {
                Id = recollection.Id,
                UserId = recollection.UserId,
                CreatedAt = recollection.CreatedAt,
                DownloadLink = recollection.DownloadLink
            };
            recollectionResponse.Add(responseDto);
        }
        
        var response = new BaseApiResponse<IEnumerable<RecollectionResponseDto>> { Payload = recollectionResponse };
        return Ok(response);
    }
}