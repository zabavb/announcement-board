using AuthApi.Models;
using AuthApi.Repositories.Interfaces;

namespace AuthApi.Repositories;

public class AuthRepository : IAuthRepository
{
    public Task<User?> GetUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task RegisterAsync(User user)
    {
        throw new NotImplementedException();
    }
}