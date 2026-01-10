namespace Mappevurdering1.Models;

/// <summary>
/// Et objekt for å lagre resultatene. Blir mer organisert og vi slipper å ha en haug med lister å holde styr på
/// </summary>
public class SearchResult
{
    public int ArraySize { get; set; }
    public TypeEnum TypeEnum { get; set; }
    public double LinearTimeMs { get; set; }
    public double BinaryTimeMs { get; set; }
    public double SortTimeMs { get; set; }
    public int LinearResult { get; set; }
    public int BinaryResult { get; set; }
    public bool IsFound { get; set; }
}