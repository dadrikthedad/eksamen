using System.ComponentModel.DataAnnotations;

namespace FredrikMageeEmne7Arbkrav.DTOs.Requests;

/// <summary>
/// En DTO som brukes når vi oppretter en bok. Vi validerer hvert felt med attributene vi har opprettet over feltene.
/// PublicationYear er satt til 2025
/// </summary>
public class CreateBookRequest
{
    [Required(ErrorMessage = "The title of the book is required.")]
    [MaxLength(200, ErrorMessage = "The title can't be longer than 200 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Author of the book is required.")]
    [MaxLength(100, ErrorMessage = "Author can't be longer than 100 characters")]
    public string Author { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Publication Year is required.")]
    [Range(0, 2030 , ErrorMessage = "Publication Year must be higher than 0 and lower than 2030")]
    public int PublicationYear { get; set; }
    
    [Required(ErrorMessage = "ISBN is required.")]
    [StringLength(25, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 25 characters")]
    public string ISBN { get; set; } = string.Empty;
    
    [Required(ErrorMessage ="In Stock is required.")]
    public int InStock { get; set; }
}