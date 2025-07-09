using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApi.Models;
using AuthApi.Services.Interfaces;
using Google.Apis.Auth;
using Library.Models.Auth;
using Library.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService service, IOptions<JwtSettings> jwtOptions, IConfiguration config)
    : ControllerBase
{
    private readonly IAuthService _service = service;
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    private readonly IConfiguration _config = config;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _service.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var token = GenerateJwtToken(user);
        return Ok(new AuthResponse(token, _jwtSettings.ExpiresInDays, user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.RegisterAsync(request.FullName, request.Email, request.Password);
        return Created(nameof(Register), null);
    }

    [HttpPost("oauth")]
    public async Task<IActionResult> OAuth([FromBody] OAuthRequest request)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_config["OAuth:ClientId"]]
        };

        var user = await _service.OAuthAsync(request.Token, settings);

        if (user.Id.Equals(Guid.Empty))
            return Ok(user);

        var token = GenerateJwtToken(user);
        return Ok(new AuthResponse(token, _jwtSettings.ExpiresInDays, user));
    }

    [NonAction]
    private string GenerateJwtToken(UserDto user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpiresInDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}