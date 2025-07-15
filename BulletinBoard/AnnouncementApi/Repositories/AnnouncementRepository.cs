using System.Data;
using AnnouncementApi.Repositories.Interfaces;
using Dapper;
using Library.Models;

namespace AnnouncementApi.Repositories;

public class AnnouncementRepository(IDbConnection db) : IAnnouncementRepository
{
    private readonly IDbConnection _db = db;
    private const string QueriesDir = "Data/Queries/";

    public async Task<ICollection<Announcement>> GetAsync(Guid? categoryId, Guid? subcategoryId)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetAllAnnouncements.sql");

        var result = await _db.QueryAsync<Announcement>(
            sql,
            new { SubcategoryId = subcategoryId, CategoryId = categoryId }
        );

        return result.ToList();
    }

    public async Task<Announcement?> GetByIdAsync(Guid id)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "GetAnnouncementById.sql");

        var result = await _db.QueryFirstOrDefaultAsync<Announcement>(
            sql,
            new { Id = id }
        );
        return result;
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
            announcement.SubcategoryId
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
            announcement.SubcategoryId
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        var sql = await File.ReadAllTextAsync(QueriesDir + "DeleteAnnouncement.sql");

        await _db.ExecuteAsync(sql, new { Id = id });
    }
}