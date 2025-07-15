using AnnouncementApi.Services.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncementApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsController(IAnnouncementService service) : ControllerBase
{
    private readonly IAnnouncementService _service = service;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? categoryId, [FromQuery] Guid? subcategoryId)
    {
        var announcements = await _service.GetAsync(categoryId, subcategoryId);
        return Ok(announcements);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Announcement>> GetById(Guid id)
    {
        if (id == Guid.Empty)
            return NotFound("ID was not provided.");

        var announcement = await _service.GetByIdAsync(id);
        return Ok(announcement);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Announcement announcement)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.CreateAsync(announcement);
        return CreatedAtAction(nameof(Get), new { announcement.Id }, announcement);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Announcement announcement)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (id != announcement.Id)
            return BadRequest("User ID in the URL does not match the ID in the body.");

        await _service.UpdateAsync(announcement);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<NoContentResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}