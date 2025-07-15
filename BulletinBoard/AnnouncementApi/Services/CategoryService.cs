using AnnouncementApi.Repositories.Interfaces;
using AnnouncementApi.Services.Interfaces;
using Library.Models;
using Library.Models.Categories;
using Library.Models.Dto;

namespace AnnouncementApi.Services;

public class CategoryService(ICategoryRepository repository, ILogger<ICategoryService> logger) : ICategoryService
{
    private readonly ICategoryRepository _repository = repository;
    private readonly ILogger<ICategoryService> _log = logger;

    public async Task<ICollection<CategoryWithSubcategoryDto>> GetAsync()
    {
        var categories = await _repository.GetAsync();
        _log.LogInformation("Successfully retrieved categories.");
        
        return categories;
    }
}