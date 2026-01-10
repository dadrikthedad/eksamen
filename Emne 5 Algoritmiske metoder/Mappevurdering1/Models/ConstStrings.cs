namespace Mappevurdering1.Models;

/// <summary>
/// Lagrer strings som brukes i flere klasser, samt filstiene for å ha det organisert et sted
/// </summary>
public static class ConstStrings
{
    public const string Separator =
        $"---------------------------------------------------------------------------------------------------------" +
        $"-----------------------------------------------------------------------------";
    
    public const string DoubleSeparator =
        $"=========================================================================================================" +
        $"=============================================================================";
    
    public const int SmallWidth = -12;
    public const int BigWidth = -20;

    public const string NotFound = "Not found";
    public const string Empty = "------";
    
    public const string AllResultsFilePath = "results/allResults.txt";
    public const string ArrayResultsFilePath = "results/arrayResults.txt";
    public const string ListResultsFilePath = "results/listResults.txt";
    public const string LinkedListResultsFilePath = "results/linkedListResults.txt";
    
}