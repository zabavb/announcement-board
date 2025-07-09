using AuthApi.Models;

namespace AuthApi.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetUserAsync(string email);
    Task RegisterAsync(User user);
}