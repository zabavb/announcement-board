using Library.Models;

namespace AnnouncementApi.Services.Interfaces;

public interface ICategoryService
{
    public Task<ICollection<Category>> GetAsync();
}