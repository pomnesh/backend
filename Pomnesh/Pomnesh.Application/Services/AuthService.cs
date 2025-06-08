using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pomnesh.Application.Exceptions;
using Pomnesh.Application.Interfaces;
using Pomnesh.Application.Models;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Serilog;

namespace Pomnesh.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = Log.ForContext<AuthService>();
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        _logger.Information("Attempting login for user: {Username}", request.Username);

        var user = await _userRepository.GetByUsername(request.Username);

        if (user == null)
        {
            _logger.Warning("Login failed: User {Username} not found", request.Username);
            throw new AuthenticationError("Invalid username or password");
        }

        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.Warning("Login failed: Invalid password for user {Username}", request.Username);
            throw new AuthenticationError("Invalid username or password");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.Update(user);

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user);
        _logger.Information("Login successful for user: {Username}", request.Username);

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        _logger.Information("Attempting registration for user: {Username}", request.Username);

        var existingUser = await _userRepository.GetByUsername(request.Username);
        if (existingUser != null)
        {
            _logger.Warning("Registration failed: Username {Username} already exists", request.Username);
            throw new AuthenticationError("Username already exists");
        }

        var existingEmail = await _userRepository.GetByEmail(request.Email);
        if (existingEmail != null)
        {
            _logger.Warning("Registration failed: Email {Email} already exists", request.Email);
            throw new AuthenticationError("Email already exists");
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.Add(user);
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(user);
        _logger.Information("Registration successful for user: {Username}", request.Username);

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration"));
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Token validation failed");
            return false;
        }
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        _logger.Information("Attempting to refresh token");

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration"));
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);
            
            if (validatedToken is not JwtSecurityToken jwtToken)
            {
                _logger.Warning("Token refresh failed: Invalid token format");
                throw new AuthenticationError("Invalid token format");
            }


            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                _logger.Warning("Token refresh failed: Token has expired");
                throw new AuthenticationError("Refresh token has expired");
            }

            var tokenType = jwtToken.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
            if (tokenType != "refresh")
            {
                _logger.Warning("Token refresh failed: Invalid token type. Refresh token expected.");
                throw new AuthenticationError("Invalid token type. Refresh token expected.");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.Warning("Token refresh failed: User ID not found in token");
                throw new AuthenticationError("Invalid refresh token");
            }

            var user = await _userRepository.GetById(int.Parse(userId));
            if (user == null)
            {
                _logger.Warning("Token refresh failed: User {UserId} not found", userId);
                throw new AuthenticationError("User not found");
            }

            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user);
            _logger.Information("Token refresh successful for user: {Username}", user.Username);

            return new AuthResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Username = user.Username,
                Email = user.Email
            };
        }
        catch (SecurityTokenExpiredException)
        {
            _logger.Warning("Token refresh failed: Token has expired");
            throw new AuthenticationError("Refresh token has expired");
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Token refresh failed");
            throw new AuthenticationError("Invalid refresh token");
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration"));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("token_type", "access")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration"));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("token_type", "refresh")
            }),
            Expires = DateTime.UtcNow.AddMonths(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
} 