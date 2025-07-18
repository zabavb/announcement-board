﻿using Library.Models;

namespace AnnouncementApi.Services.Interfaces;

public interface IAnnouncementService
{
    public Task<ICollection<Announcement>> GetAsync(Guid? categoryId, Guid? subcategory);
    public Task<Announcement?> GetByIdAsync(Guid id);
    public Task CreateAsync(Announcement announcement);
    public Task UpdateAsync(Announcement announcement);
    public Task DeleteAsync(Guid id);
}