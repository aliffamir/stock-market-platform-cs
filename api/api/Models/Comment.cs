namespace api.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    // Foreign Key
    public int? StockId { get; set; }
    // Navigation property
    public Stock? Stock { get; set; }
}