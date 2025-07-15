using System.Data;
using AnnouncementApi.Repositories.Interfaces;
using Dapper;
using Library.Models.Categories;
using Library.Models.Dto;

namespace AnnouncementApi.Repositories;

public class CategoryRepository(IDbConnection db) : ICategoryRepository
{
    private readonly IDbConnection _db = db;
    private const string QueriesDir = "Data/Queries/";

    public async Task<ICollection<CategoryWithSubcategoryDto>> GetAsync()
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetAllCategoriesWithSubcategories.sql");

        var categories = await _db.QueryAsync<CategoryWithSubcategoryDto>(sql);

        return categories.ToList();
    }
}