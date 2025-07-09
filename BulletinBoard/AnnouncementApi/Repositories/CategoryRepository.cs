using AnnouncementApi.Repositories.Interfaces;
using Library.Models;

namespace AnnouncementApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public Task<ICollection<Category>> GetAsync()
    {
        throw new NotImplementedException();
    }
}