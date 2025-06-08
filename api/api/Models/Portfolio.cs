using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

// join table model
[Table("Portfolio")]
public class Portfolio
{
   public string AppUserId { get; set; } 
   public int StockId { get; set; }
   public AppUser AppUser { get; set; }
   public Stock Stock { get; set; }
}