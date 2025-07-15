using Library.Models;
using Library.Models.Categories;
using Library.Models.Dto;

namespace AnnouncementApi.Repositories.Interfaces;

public interface ICategoryRepository
{
    public Task<ICollection<CategoryWithSubcategoryDto>> GetAsync();
}