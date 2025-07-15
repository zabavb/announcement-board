using Library.Models;
using Library.Models.Categories;
using Library.Models.Dto;

namespace AnnouncementApi.Services.Interfaces;

public interface ICategoryService
{
    public Task<ICollection<CategoryWithSubcategoryDto>> GetAsync();
}