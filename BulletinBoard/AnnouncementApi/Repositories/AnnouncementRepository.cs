using System.Data;
using AnnouncementApi.Repositories.Interfaces;
using Dapper;
using Library.Models;

namespace AnnouncementApi.Repositories;

public class AnnouncementRepository(IDbConnection db) : IAnnouncementRepository
{
    private readonly IDbConnection _db = db;
    private const string QueriesDir = "Data/Queries/";

    public async Task<ICollection<Announcement>> GetAsync(Guid? subcategoryId)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetAllAnnouncementsWithCategory.sql");

        var result = await _db.QueryAsync<Announcement, Category, Category, Announcement>(
            sql,
            (announcement, category, subcategory) =>
            {
                announcement.Category = category;
                announcement.Subcategory = subcategory;
                return announcement;
            },
            new { SubcategoryId = subcategoryId },
            splitOn: "CategoryId,SubcategoryId"
        );

        return result.ToList();
    }

    public async Task CreateAsync(Announcement announcement)
    {
        if (announcement is null)
            throw new ArgumentNullException(nameof(announcement));
        
        var sql = await File.ReadAllTextAsync(QueriesDir + "InsertAnnouncement.sql");

        await _db.ExecuteAsync(sql, new
        {
            announcement.Id,
            announcement.Title,
            announcement.Description,
            announcement.CreatedDate,
            Status = (int)announcement.Status,
            CategoryId = announcement.Category.Id,
            SubcategoryId = announcement.Subcategory.Id
        });
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        if (announcement == null)
            throw new ArgumentNullException(nameof(announcement));
        
        var sql = await File.ReadAllTextAsync(QueriesDir + "UpdateAnnouncement.sql");

        await _db.ExecuteAsync(sql, new
        {
            announcement.Id,
            announcement.Title,
            announcement.Description,
            Status = (int)announcement.Status,
            CategoryId = announcement.Category?.Id,
            SubcategoryId = announcement.Subcategory?.Id
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "DeleteAnnouncement.sql");

        await _db.ExecuteAsync(sql, new { Id = id });
    }
}