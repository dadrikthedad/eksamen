using Mappevurdering1.Analysis;
using Mappevurdering1.Models;

namespace Mappevurdering1.Experiments;

/// <summary>
/// Metodene til denne klassen lagrer resultatet til konsoll og fil, etter det er formatert til en tabell
/// </summary>
public static class ResultSaver
{
    /// <summary>
    /// Oppretter en rad til resultattabellen i konsollen og til fil. Samt at vi lagrer det til en slutt analyse
    /// av hver array-lengde
    /// </summary>
    /// <param name="searchResult">Resultatet etter et søk</param>
    public static void SaveResult(SearchResult searchResult)
    {
        var resultString = ResultTableBuilder.ArrangeTableRow(searchResult);
        
        // Lagrer SearchResulten for å kunne analyseres etter hver array-størrelse og etter alle eksperimentene
        ResultsAnalyzer.SearchResults.Add(searchResult);
        
        // Printer resultat-stringen til konsollen
        Console.WriteLine(resultString);
        
        // Lagrer resultatene til fil
        FileWriter.SaveResultToFile(resultString, searchResult.TypeEnum);
    }
    
}