using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Library.Data;

public class DataSeeder(IDbConnection db, ILogger<DataSeeder> logger)
{
    private readonly IDbConnection _db = db;
    private readonly ILogger<DataSeeder> _log = logger;

    public async Task SeedAsync()
    {
        var existingCategories = await _db.QueryAsync<Guid>("SELECT Id FROM Categories");
        if (existingCategories.Any())
        {
            _log.LogInformation("Skipping seeding: Categories already exist.");
            return;
        }

        _log.LogInformation("Seeding categories and subcategories.");

        // Step 1: Seed Categories
        var categories = new Dictionary<string, Guid>
        {
            { "Home appliances", Guid.NewGuid() },
            { "Computer equipment", Guid.NewGuid() },
            { "Smartphones", Guid.NewGuid() },
            { "Other", Guid.NewGuid() }
        };

        foreach (var (name, id) in categories)
        {
            await _db.ExecuteAsync(
                "INSERT INTO Categories (Id, Name) VALUES (@Id, @Name)",
                new { Id = id, Name = name });
        }

        // Step 2: Seed Subcategories
        var subcategories = new List<(string Name, Guid ParentId)>
        {
            ("Refrigerators", categories["Home appliances"]),
            ("Washing Machines", categories["Home appliances"]),
            ("Boilers", categories["Home appliances"]),
            ("Ovens", categories["Home appliances"]),
            ("Range hoods", categories["Home appliances"]),
            ("Microwave ovens", categories["Home appliances"]),
            ("PCs", categories["Computer equipment"]),
            ("Laptops", categories["Computer equipment"]),
            ("Monitors", categories["Computer equipment"]),
            ("Printers", categories["Computer equipment"]),
            ("Scanners", categories["Computer equipment"]),
            ("Android smartphones", categories["Smartphones"]),
            ("iOS/Apple smartphones", categories["Smartphones"]),
            ("Clothing", categories["Other"]),
            ("Shoes", categories["Other"]),
            ("Accessories", categories["Other"]),
            ("Sports Equipment", categories["Other"]),
            ("Toys", categories["Other"])
        };

        foreach (var (name, parentId) in subcategories)
        {
            await _db.ExecuteAsync(
                "INSERT INTO Subcategories (Id, Name, ParentId) VALUES (@Id, @Name, @ParentId)",
                new { Id = Guid.NewGuid(), Name = name, ParentId = parentId });
        }

        _log.LogInformation("Seeding complete.");
    }
}