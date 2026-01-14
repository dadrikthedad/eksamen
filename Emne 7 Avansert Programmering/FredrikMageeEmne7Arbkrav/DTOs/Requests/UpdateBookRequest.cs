using System.ComponentModel.DataAnnotations;

namespace FredrikMageeEmne7Arbkrav.DTOs.Requests;

/// <summary>
/// Vi følger nesten samme oppsett som CreateBookRequest og databasereglene, men her er feltene nullable og ikke required.
/// Nullable slik at brukere kan kun oppdatere de feltene de ønsker og kan sende inn noen eller alle om det er ønskelig
/// </summary>
public class UpdateBookRequest
{
    [MaxLength(200, ErrorMessage = "The title can't be longer than 200 characters")]
    public string? Title { get; set; }

    [MaxLength(100, ErrorMessage = "The author can't be longer than 100 characters")]
    public string? Author { get; set; }
    
    [Range(0, 2030, ErrorMessage = "Publication Year must be higher than 0 and lower than 2030")]
    public int? PublicationYear { get; set; }
    
    [MaxLength(25, ErrorMessage = "ISBN cannot be more than 25 characters.")]
    public string? ISBN { get; set; }
    
    public int? InStock { get; set; }
}