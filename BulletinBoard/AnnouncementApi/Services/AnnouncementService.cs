using AnnouncementApi.Repositories.Interfaces;
using AnnouncementApi.Services.Interfaces;
using Library.Models;

namespace AnnouncementApi.Services;

public class AnnouncementService(
    IAnnouncementRepository repository,
    ILogger<IAnnouncementService> logger
) : IAnnouncementService
{
    private readonly IAnnouncementRepository _repository = repository;
    private readonly ILogger<IAnnouncementService> _log = logger;

    public async Task<ICollection<Announcement>> GetAsync(Category? subcategory)
    {
        var announcements = await _repository.GetAsync(subcategory);
        _log.LogInformation("Successfully retrieved announcements.");

        return announcements;
    }

    public async Task CreateAsync(Announcement announcement)
    {
        await _repository.CreateAsync(announcement);
        _log.LogInformation("Successfully created announcement.");
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        await _repository.UpdateAsync(announcement);
        _log.LogInformation("Successfully updated announcement.");
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        _log.LogInformation("Successfully deleted announcement.");
    }
}