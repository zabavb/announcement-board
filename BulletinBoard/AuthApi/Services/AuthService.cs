using System.Security.Cryptography;
using System.Text;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using AuthApi.Services.Interfaces;
using AutoMapper;
using Google.Apis.Auth;
using Library.Models.Dto;

namespace AuthApi.Services;

public class AuthService(IAuthRepository repository, IMapper mapper, ILogger<IAuthService> logger) : IAuthService
{
    private readonly IAuthRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IAuthService> _log = logger;

    public async Task<UserDto?> AuthenticateAsync(string email, string password)
    {
        var user = await _repository.GetUserAsync(email);

        if (user == null) return null;
        if (user.Password == null)
            throw new InvalidOperationException("Password is missing.");

        var hashedPassword = HashPassword(password);
        if (user.Password != hashedPassword)
        {
            _log.LogWarning("User [{email}] is not authenticated.", email);
            return null;
        }

        _log.LogInformation("User [{email}] is authenticated.", email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task RegisterAsync(string fullName, string email, string password)
    {
        var hashedPassword = HashPassword(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName,
            Email = email,
            Password = hashedPassword
        };

        await _repository.RegisterAsync(user);
        _log.LogInformation("Successful user registration.");
    }

    public async Task<UserDto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

        var user = _mapper.Map<UserDto>(await _repository.GetUserAsync(payload.Email)) ?? new UserDto
        {
            Id = Guid.Empty, // TODO: If ID is empty Guid -> Redirect to registration form
            FullName = payload.Name,
            Email = payload.Email,
        };

        _log.LogInformation("OAuthAsync() => return {user}", user);
        return user;
    }

    private static string HashPassword(string password)
    {
        var encoded = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(encoded);
        var result = Convert.ToBase64String(hash);
        return result.Substring(0, result.Length - 1);
    }
}