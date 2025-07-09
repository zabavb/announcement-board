using AnnouncementApi.Repositories.Interfaces;
using AnnouncementApi.Services.Interfaces;
using Library.Models;

namespace AnnouncementApi.Services;

public class CategoryService(ICategoryRepository repository, ILogger<ICategoryService> logger) : ICategoryService
{
    private readonly ICategoryRepository _repository = repository;
    private readonly ILogger<ICategoryService> _log = logger;

    public async Task<ICollection<Category>> GetAsync()
    {
        var categories = await _repository.GetAsync();
        _log.LogInformation("Successfully retrieved categories.");
        
        return categories;
    }
}