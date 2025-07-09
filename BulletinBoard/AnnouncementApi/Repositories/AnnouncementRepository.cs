using AnnouncementApi.Repositories.Interfaces;
using Library.Models;

namespace AnnouncementApi.Repositories;

public class AnnouncementRepository : IAnnouncementRepository
{
    public Task<ICollection<Announcement>> GetAsync(Category? subcategory)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Announcement announcement)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Announcement announcement)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}