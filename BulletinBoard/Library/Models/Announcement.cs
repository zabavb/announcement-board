namespace Library.Models;

public class Announcement
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public Status Status { get; set; } = Status.Active;

    public Guid SubcategoryId { get; set; }
}