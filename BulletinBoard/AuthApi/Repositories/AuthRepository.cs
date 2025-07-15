using System.Data;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using Dapper;

namespace AuthApi.Repositories;

public class AuthRepository(IDbConnection db) : IAuthRepository
{
    private readonly IDbConnection _db = db;
    private const string QueriesDir = "Data/Queries/";

    public async Task<User?> GetUserAsync(string email)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetUserByEmail.sql");

        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task RegisterAsync(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var sql = await File.ReadAllTextAsync(QueriesDir + "InsertUser.sql");

        await _db.ExecuteAsync(sql, new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.Password
        });
    }
}