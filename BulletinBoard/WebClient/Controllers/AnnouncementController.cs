using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Services.Interfaces;

namespace WebClient.Controllers;

[Authorize]
public class AnnouncementController(IAnnouncementService service) : Controller
{
    private readonly IAnnouncementService _service = service;

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index(Guid? categoryId, Guid? subcategoryId)
    {
        var announcements = await _service.GetAnnouncementsAsync(categoryId, subcategoryId);
        var categories = await _service.GetCategoriesAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedSubcategoryId = subcategoryId;
        
        return View(announcements);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _service.GetCategoriesAsync();
        return View(new Announcement());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Announcement announcement)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _service.GetCategoriesAsync();
            return View(announcement);
        }

        if (await _service.CreateAsync(announcement))
        {
            TempData["Success"] = "Announcement created successfully.";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Failed to create announcement.";
        ViewBag.Categories = await _service.GetCategoriesAsync();
        return View(announcement);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var announcement = await _service.GetByIdAsync(id);
        if (announcement == null)
        {
            TempData["Error"] = "Announcement not found.";
            return RedirectToAction("Index");
        }

        ViewBag.Categories = await _service.GetCategoriesAsync();
        return View(announcement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Announcement announcement)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _service.GetCategoriesAsync();
            return View(announcement);
        }

        if (await _service.UpdateAsync(announcement))
        {
            TempData["Success"] = "Announcement updated successfully.";
            return RedirectToAction("Index");
        }

        TempData["Error"] = "Failed to update announcement.";
        ViewBag.Categories = await _service.GetCategoriesAsync();
        return View(announcement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (await _service.DeleteAsync(id))
            TempData["Success"] = "Announcement deleted successfully.";
        else
            TempData["Error"] = "Failed to delete announcement.";

        return RedirectToAction("Index");
    }
}