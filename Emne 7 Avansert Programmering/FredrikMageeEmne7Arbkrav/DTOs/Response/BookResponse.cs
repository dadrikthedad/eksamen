namespace FredrikMageeEmne7Arbkrav.DTOs.Request.Response;

/// <summary>
/// BookResponse DTO for å ikke returnere Book-modellen slik jeg forstår er best practice
/// Ved å bruke en response/DTO så får vi mer kontroll over hvilken informasjon vi sender til frontend
/// </summary>
public class BookResponse
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;
    
    public int PublicationYear { get; set; }
    
    public string ISBN { get; set; } = string.Empty;
    
    public int InStock { get; set; }
}