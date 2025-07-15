using Library.Models.Auth;
using Library.Models.Dto;
using WebClient.Models;
using WebClient.Models.ViewModels;

namespace WebClient.Services.Interfaces;

public interface IAccountService
{
    public Task SignInUser(UserDto user, string jwt);
    public Task<AuthResponse?> LoginAsync(LoginViewModel model);
    public Task<bool> RegisterAsync(RegisterViewModel model);
    public Task<(AuthResponse? Auth, UserDto? Fallback)> OAuthLoginAsync(string token);
}