namespace Library.Models.Dto;

public class CategoryWithSubcategoryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid SubcategoryId { get; set; }
    public string SubcategoryName { get; set; } = string.Empty;
}