using System.Text.Json.Serialization;

namespace FredrikMageeArbkrav1Emne6.Models;

public class University
{
    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
    
    [JsonPropertyName("alpha_two_code")]
    public string CountryCode { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("web_pages")]
    public List<string> WebPages { get; set; } = [];
    
    [JsonPropertyName("domains")]
    public List<string> Domains { get; set; } = [];
}