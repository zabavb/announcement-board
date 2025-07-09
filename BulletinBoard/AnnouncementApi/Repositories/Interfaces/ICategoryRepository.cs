using Library.Models;

namespace AnnouncementApi.Repositories.Interfaces;

public interface ICategoryRepository
{
    public Task<ICollection<Category>> GetAsync();
}