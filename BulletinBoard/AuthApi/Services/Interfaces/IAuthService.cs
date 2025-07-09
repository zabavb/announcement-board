using Google.Apis.Auth;
using Library.Models.Dto;

namespace AuthApi.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto?> AuthenticateAsync(string email, string password);
    Task RegisterAsync(string fullName, string email, string password);
    Task<UserDto> OAuthAsync(string token, GoogleJsonWebSignature.ValidationSettings settings);
}