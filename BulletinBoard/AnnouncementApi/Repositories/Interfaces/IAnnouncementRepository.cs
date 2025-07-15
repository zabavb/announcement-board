using Library.Models;

namespace AnnouncementApi.Repositories.Interfaces;

public interface IAnnouncementRepository
{
    public Task<ICollection<Announcement>> GetAsync(Guid? categoryId, Guid? subcategoryId);
    public Task<Announcement?> GetByIdAsync(Guid id);
    public Task CreateAsync(Announcement announcement);
    public Task UpdateAsync(Announcement announcement);
    public Task DeleteAsync(Guid id);
}