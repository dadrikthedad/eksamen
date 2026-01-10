using Mappevurdering1.Models;

namespace Mappevurdering1.Analysis;

/// <summary>
/// Denne klassen arrangerer resultatene fra hver array-størrelse i et formatert og ryddig format
/// for å iterere igjennom resultatene etter vi har kjørt ferdig alle eksperimentene
/// </summary>
public static class ResultsAnalyzer
{
    // ============================================= Felt =============================================
    public static readonly List<SearchResult> SearchResults = [];
    private static readonly List<string?> ResultsToAnalyze = [];
    
    
    // ============================================= Metoder =============================================
    
    /// <summary>
    /// Denne metoden arrangerer resultet i en liste med formaterte strings for å vise en tabell og noen setninger
    /// med analysen av resultatene vi har fått fra kjøringen av eksperimentene
    /// </summary>
    /// <param name="arraySize">Størrelsen på arrayet for å samle resultatene til hver array størrelse</param>
    public static void ArrangeAnalyzeResults(int arraySize)
    {
        // Henter ut alle gjennomsnittene inn i et AverageResults-objekt for å gjøre koden mer lesbar
        var averageResults = ResultAverageCaluclator.GetAveragesFromResults();
        
        // Setningn og delelinjen som kommer før analyse-tabellen
        ResultsToAnalyze.Add($"Analyse av lister på {arraySize} elementer: ");
        ResultsToAnalyze.Add(ConstStrings.Separator);
        
        // Analyse tabellen
        ResultsToAnalyze.AddRange(AnalyzeResultBuilder.CreateAnalyzeTable(averageResults));
        ResultsToAnalyze.Add(ConstStrings.Separator);
        
        // Analyse-delen under tabellen
        ResultsToAnalyze.AddRange(AnalyzeResultBuilder.CreateResultAnalyze(averageResults));
        
        ResultsToAnalyze.Add(ConstStrings.Separator + "\n");
        
        // Da er vi ferdig med en array-størrelse og kan resette listen
        SearchResults.Clear();
    }
    
    /// <summary>
    /// Iterer igjennom de lagrede resultat-strengene for å vise detaljert informasjon angående søkende til hver array
    /// størrelse
    /// </summary>
    public static void RunAnalyze()
    {
        var index = 0;
        foreach (var str in ResultsToAnalyze)
        {
            Console.WriteLine(str);
            index++;
            // Hver kjøring av ArrangeAnalyzeResults gir 17 setninger. Slik kan vi hente ut kun setningene tilhørende
            // hver array størrelse
            if (index % 17 == 0 && index < ResultsToAnalyze.Count)
            {
                Console.WriteLine("\nAnalyse utført. Trykk en tast for å se analyse av neste størrelse.\n");
                Console.ReadKey();
                Console.WriteLine(ConstStrings.DoubleSeparator);
            }
        }
    }
}