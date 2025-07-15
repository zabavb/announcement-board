using Library.Models.Categories;
using Library.Models.Dto;
using WebClient.Services.Interfaces;

namespace WebClient.Services;

using System.Net.Http.Headers;
using System.Text.Json;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AnnouncementService(
    IHttpClientFactory factory,
    IHttpContextAccessor contextAccessor,
    ILogger<AnnouncementService> log)
    : IAnnouncementService
{
    private readonly IHttpClientFactory _factory = factory;
    private readonly IHttpContextAccessor _httpAccessor = contextAccessor;
    private readonly ILogger<AnnouncementService> _log = log;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    private HttpClient CreateClient()
    {
        var client = _factory.CreateClient("AnnouncementApi");
        var token = _httpAccessor.HttpContext?.Request.Cookies["AccessToken"];
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync(Guid? categoryId, Guid? subcategoryId)
    {
        try
        {
            var client = CreateClient();
            // Forming a query
            var query = new List<string>();
            if (subcategoryId.HasValue)
                query.Add($"subcategoryId={subcategoryId}");
            if (categoryId.HasValue)
                query.Add($"categoryId={categoryId}");

            var uri = "api/announcements";
            if (query.Count > 0)
                uri += "?" + string.Join("&", query);
            
            // Sending request
            var data = await client.GetFromJsonAsync<List<Announcement>>(uri, _options);
            return data ?? [];
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to fetch announcements.");
            return [];
        }
    }

    public async Task<Announcement?> GetByIdAsync(Guid id)
    {
        try
        {
            var client = CreateClient();
            return await client.GetFromJsonAsync<Announcement>($"api/announcements/{id}", _options);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to get announcement by ID.");
            return null;
        }
    }

    public async Task<bool> CreateAsync(Announcement announcement)
    {
        try
        {
            var client = CreateClient();
            var res = await client.PostAsJsonAsync("api/announcements", announcement);
            return res.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to create announcement.");
            return false;
        }
    }

    public async Task<bool> UpdateAsync(Announcement announcement)
    {
        try
        {
            var client = CreateClient();
            var res = await client.PutAsJsonAsync($"api/announcements/{announcement.Id}", announcement);
            return res.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to update announcement.");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var client = CreateClient();
            var res = await client.DeleteAsync($"api/announcements/{id}");
            return res.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to delete announcement.");
            return false;
        }
    }

    public async Task<List<CategoryWithSubcategoryDto>> GetCategoriesAsync()
    {
        try
        {
            var client = CreateClient();
            var categories =
                await client.GetFromJsonAsync<List<CategoryWithSubcategoryDto>>("api/categories", _options);
            return categories ?? [];
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to load categories.");
            return [];
        }
    }
}