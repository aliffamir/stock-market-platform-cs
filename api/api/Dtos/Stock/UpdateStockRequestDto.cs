using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock;

public class UpdateStockRequestDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters.")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(20, ErrorMessage = "Company Name cannot exceed 20 characters.")]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000000000)]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100)]
    public decimal LastDividend { get; set; }
    [Required]
    [MaxLength(20, ErrorMessage = "Industry cannot exceed 20 characters.")]
    public string Industry { get; set; } = string.Empty;
    [Required]
    [Range(1, 5000000000000)]
    public long MarketCap { get; set; }
}