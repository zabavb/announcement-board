using AnnouncementApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryService service) : ControllerBase
{
    private readonly ICategoryService _service = service;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categories = await _service.GetAsync();
        return Ok(categories);
    }
}