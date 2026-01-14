using System.ComponentModel.DataAnnotations;

namespace FredrikMageeEmne7Arbkrav.Models;
/// <summary>
/// Book-modellen med attributer. Vi oppretter databasen og tabellen via EFCore. Tabellen vil se ut som Book-modellen
/// med egenskapene som kolonner. Begrunnelsen til attributene finnes i Readme.md-filen
/// </summary>
public class Book
{
    // Id blir automatisk satt som PRIMARY KEY av EFCore
    public int Id { get; set; }
    
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Author { get; set; } = string.Empty;
    
    // Range for å sette at bøker kan være utgitt mellom 0-2030
    [Required]
    [Range(0, 2030)]
    public int PublicationYear { get; set; }
    
    [Required]
    [StringLength(25, MinimumLength = 10)]
    public string ISBN { get; set; } = string.Empty;
    
    [Required]
    public int InStock { get; set; }
}