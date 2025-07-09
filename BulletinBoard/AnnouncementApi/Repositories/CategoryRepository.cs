using System.Data;
using AnnouncementApi.Repositories.Interfaces;
using Dapper;
using Library.Models;

namespace AnnouncementApi.Repositories;

public class CategoryRepository(IDbConnection db) : ICategoryRepository
{
    private readonly IDbConnection _db = db;
    private const string QueriesDir = "Data/Queries/";

    public async Task<ICollection<Category>> GetAsync()
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetAllCategories.sql");

        var categories = await _db.QueryAsync<Category>(sql);

        return categories.ToList();
    }
}