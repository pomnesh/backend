using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.API.Responses;
using Swashbuckle.AspNetCore.Annotations;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Pomnesh.API.Attributes;
using System.Security.Claims;
using Pomnesh.API.Dto;

namespace Pomnesh.API.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")] // 5 requests per minute
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(new BaseApiResponse<AuthResponse> { Payload = response });
    }

    [HttpPost("register")]
    [EnableRateLimiting("register")] // 3 requests per minute
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(new BaseApiResponse<AuthResponse> { Payload = response });
    }

    [HttpGet("validate")]
    [Authorize]
    [EnableRateLimiting("validate")] // 30 requests per minute
    public IActionResult ValidateToken()
    {
        return Ok(new BaseApiResponse<bool> { Payload = true });
    }

    [HttpPost("refresh")]
    [EnableRateLimiting("refresh")] // 10 requests per minute
    [SkipJwtMiddleware]
    public async Task<IActionResult> RefreshToken([FromHeader(Name = "Authorization")] string token)
    {
        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "Invalid token format" });
        }

        var tokenValue = token.Substring("Bearer ".Length);
        var response = await _authService.RefreshTokenAsync(tokenValue);
        return Ok(new BaseApiResponse<AuthResponse> { Payload = response });
    }

    [HttpGet("me")]
    [Authorize]
    [EnableRateLimiting("validate")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "User not found" });
        }

        var user = await _userService.Get(long.Parse(userId));
        return Ok(new BaseApiResponse<UserResponse> { Payload = user });
    }
}