using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.API.Responses;
using Swashbuckle.AspNetCore.Annotations;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.RateLimiting;

namespace Pomnesh.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")] // 5 requests per minute
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(new BaseApiResponse<AuthResponse> { Payload = response });
    }

    [HttpPost("register")]
    [Obsolete("This endpoint is deprecated. Admin use only.")]
    [EnableRateLimiting("register")] // 3 requests per minute
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(new BaseApiResponse<AuthResponse> { Payload = response });
    }

    [HttpPost("validate")]
    [EnableRateLimiting("validate")] // 30 requests per minute
    public async Task<IActionResult> ValidateToken([FromHeader(Name = "Authorization")] string token)
    {
        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "Invalid token format" });
        }

        var tokenValue = token.Substring("Bearer ".Length);
        var isValid = await _authService.ValidateTokenAsync(tokenValue);
        
        if (!isValid)
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "Invalid token" });
        }

        return Ok(new BaseApiResponse<bool> { Payload = true });
    }
}