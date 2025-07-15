using Library.Models;
using Library.Models.Dto;

namespace WebClient.Services.Interfaces;

public interface IAnnouncementService
{
    Task<List<Announcement>> GetAnnouncementsAsync(Guid? categoryId, Guid? subcategoryId);
    Task<Announcement?> GetByIdAsync(Guid id);
    Task<bool> CreateAsync(Announcement announcement);
    Task<bool> UpdateAsync(Announcement announcement);
    Task<bool> DeleteAsync(Guid id);
    Task<List<CategoryWithSubcategoryDto>> GetCategoriesAsync();
}
