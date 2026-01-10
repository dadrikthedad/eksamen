using System.ComponentModel.DataAnnotations;

namespace FredrikMageeArbkrav1Emne6.Models;

public class ShoppingItem
{   
    [Required(ErrorMessage = "Varenavn må fylles ut")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Varenavn må være minst 1 tegn og ikke høyere enn 50 tegn")]
    public string Name { get; set; } = string.Empty;
    
    [Range(0, 1000000, ErrorMessage = "Antall varer må være minimum 0 og ikke høyere enn 1000000")]
    public int? Amount { get; set; } = 1;
    
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Category må være minst 1 tegn og ikke høyere enn 50 tegn")]
    public string Category { get; set; } = string.Empty;
    
    [Range(0, int.MaxValue, ErrorMessage = "Estimert pris må være 0 eller høyere")]
    public decimal? EstimatedPrice { get; set; }

    public decimal? TotalPrice => EstimatedPrice * Amount;
}