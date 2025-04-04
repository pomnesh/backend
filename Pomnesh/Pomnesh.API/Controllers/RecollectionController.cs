using Microsoft.AspNetCore.Mvc;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using Pomnesh.Application.Dto;
using Pomnesh.Application.Services;

namespace Pomnesh.API.Controllers;

[Route("api/v1/Recollection")]
[ApiController]
public class RecollectionController(RecollectionService recollectionService, UserService userService) : ControllerBase
{
    private readonly RecollectionService _recollectionService = recollectionService;
    private readonly UserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateRecollection([FromBody] RecollectionCreateDto model)
    {
        // Check is User exists
        var user = await _userService.Get(model.UserId);
        if (user == null)
            return NotFound(new { message = $"User with ID {model.UserId} not found." });
        
        int newId = await _recollectionService.Create(model);
        var response = new BaseApiResponse<int> { Payload = newId };
        return CreatedAtAction(nameof(GetRecollection), new { id = newId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecollection(long id)
    {
        var result = await _recollectionService.Get(id);
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
        var result = await _recollectionService.GetAll();
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