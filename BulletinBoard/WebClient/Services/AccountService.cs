using System.Security.Claims;
using System.Text.Json;
using Library.Models.Auth;
using Library.Models.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using WebClient.Models.ViewModels;
using WebClient.Services.Interfaces;

namespace WebClient.Services;

public class AccountService(
    IHttpClientFactory factory,
    IHttpContextAccessor contextAccessor,
    ILogger<IAccountService> log,
    IOptions<JwtSettings> jwtOptions) : IAccountService
{
    private readonly HttpClient _http = factory.CreateClient("AuthApi");
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly IHttpContextAccessor _httpAccessor = contextAccessor;
    private readonly ILogger<IAccountService> _log = log;
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task SignInUser(UserDto user, string jwt)
    {
        var expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.ExpiresInDays);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = expires
        };
        
        _httpAccessor.HttpContext?.Response.Cookies.Append("AccessToken", jwt, cookieOptions);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = expires
        };

        await _httpAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties
        );
    }

    public async Task<AuthResponse?> LoginAsync(LoginViewModel model)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", model);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthResponse>(_jsonOptions);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Login failed.");
            return null;
        }
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", model);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Registration failed.");
            return false;
        }
    }

    public async Task<(AuthResponse?, UserDto?)> OAuthLoginAsync(string token)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/auth/oauth", new OAuthRequest { Token = token });
            if (!response.IsSuccessStatusCode)
                return (null, null);

            var json = await response.Content.ReadAsStringAsync();

            var auth = JsonSerializer.Deserialize<AuthResponse>(json, _jsonOptions);

            // Check if AuthResponse is valid
            if (auth?.User != null && !string.IsNullOrEmpty(auth.Token))
            {
                return (auth, null);
            }

            // Otherwise, try to deserialize to UserDto as fallback
            var fallback = JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            return (null, fallback);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "OAuth login failed.");
            return (null, null);
        }
    }
}