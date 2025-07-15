namespace Library.Models.Categories;

public class Subcategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ParentId { get; set; }
}