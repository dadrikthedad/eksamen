using Mappevurdering1.Models;

namespace Mappevurdering1.Experiments;

/// <summary>
/// Klassen som bygger og formaterer resultat-tabellen som viser alle eksperimentene i konsollen
/// </summary>
public static class ResultTableBuilder
{
    // ====================================== Metoder for å sette opp tabellen ======================================
    /// <summary>
    /// Setter opp resultatene utifra om et binært søk er gjort, om søkemålet var funnet, og kaller på
    /// CreateTableRow for å opprette en rad i tabellen til senere bruk
    /// </summary>
    /// <param name="searchResult">Resultatet etter et søk</param>
    /// <returns></returns>
    public static string ArrangeTableRow(SearchResult searchResult)
    {
        // Setter default verdier som LinkedList bruker i og med at LinkedList ikke har noen binære søk eller sortering
        var linearIndex = searchResult.IsFound ? searchResult.LinearResult.ToString() : ConstStrings.NotFound; 
        var binaryIndex = ConstStrings.Empty;
        var linearTimeMs = $"{searchResult.LinearTimeMs:F6}";
        var binaryTimeMs = ConstStrings.Empty;
        var binaryTotalMs = ConstStrings.Empty;
        var sortTimeMs = ConstStrings.Empty;
        var timesFaster = "";
        
        // Overstyrer variablene med dataen fra array og linkedlist sine resultateter
        if (searchResult.TypeEnum != TypeEnum.LinkedList)
        {
            binaryIndex = searchResult.IsFound ? searchResult.BinaryResult.ToString() : ConstStrings.NotFound;
            binaryTimeMs = $"{searchResult.BinaryTimeMs:F6} ms";
            binaryTotalMs = $"{searchResult.BinaryTimeMs + searchResult.SortTimeMs:F6} ms";
            sortTimeMs = $"{searchResult.SortTimeMs:F6} ms";
            timesFaster = TimesFaster(searchResult.LinearTimeMs, searchResult.BinaryTimeMs,
                searchResult.SortTimeMs);
        }
        
        return CreateTableRow(
            searchResult.ArraySize.ToString(), 
            linearIndex, 
            binaryIndex,
            searchResult.TypeEnum.ToString(),
            linearTimeMs,
            binaryTimeMs,
            binaryTotalMs,
            sortTimeMs,
            timesFaster
            );
    }
    
    /// <summary>
    /// Oppretter en rad i tabellen som en string med riktig formatering
    /// </summary>
    /// <param name="arraySize">Lengden på arrayet</param>
    /// <param name="linearIndex">Indexen søket fant et resultat</param>
    /// <param name="binaryIndex">Indexen søket fant et resultat</param>
    /// <param name="dataType">TypeEnum</param>
    /// <param name="linearTimeMs">Tid brukt på lineært søk</param>
    /// <param name="binaryTimeMs">Tid brukt på binært søk</param>
    /// <param name="binaryTotalMs">Tid brukt på binært søk og sortering</param>
    /// <param name="sortTimeMs">Tid brukt på sortering</param>
    /// <param name="timesFaster">Resultatet av raskest søk</param>
    /// <returns></returns>
    private static string CreateTableRow(
        string arraySize,
        string linearIndex,
        string binaryIndex,
        string dataType,
        string linearTimeMs,
        string binaryTimeMs,
        string binaryTotalMs,
        string sortTimeMs,
        string timesFaster) 
        =>
        $"{arraySize,ConstStrings.SmallWidth} " +
        $"| {linearIndex,ConstStrings.SmallWidth} " +
        $"| {binaryIndex,ConstStrings.SmallWidth} " +
        $"| {dataType,ConstStrings.SmallWidth} " +
        $"| {$"{linearTimeMs}",ConstStrings.BigWidth} " +
        $"| {$"{binaryTimeMs}",ConstStrings.BigWidth} " +
        $"| {$"{binaryTotalMs}",ConstStrings.BigWidth} " +
        $"| {$"{sortTimeMs}",ConstStrings.BigWidth}"
        + $"{timesFaster}";
    
    /// <summary>
    /// Returnerer en header-string som er formatert riktig med hjelp av CreateTableRow
    /// </summary>
    public static string GetHeader() => CreateTableRow(
        "Array length",
        "Linear index",
        "Binary index",
        "Datatype",
        "Linear Milliseconds",
        "Binary Milliseconds",
        "Binary Ms /w sort",
        "Sort Milliseconds",
        "");

    
    // ===================================== Hjelpemetode for sammenligning =====================================
    
    /// <summary>
    /// Metode som sammenligner søkene, med og uten sorteringstid inkludert, og returnerer en resultat-string deretter
    /// </summary>
    /// <param name="linearTime">Tiden brukt på et lineært søk</param>
    /// <param name="binaryTime">Tiden brukt på et binært søk</param>
    /// <param name="sortTime">Tiden brukt på å sortere</param>
    /// <returns></returns>
    private static string TimesFaster(double linearTime, double? binaryTime, double? sortTime)
    {
        string timesFaster;
        
        if (linearTime < binaryTime)
            timesFaster =
                $"Lineært søk {binaryTime / linearTime:F2} ganger raskere.";
        else
            timesFaster = 
                $"Binært søk {linearTime / binaryTime:F2} ganger raskere.";
        
        
        if (binaryTime + sortTime < linearTime)
            timesFaster +=
                $" Med sortering: Binært søk {linearTime / (binaryTime + sortTime):F2} ganger raskere.";
        else
            timesFaster +=
                $" Med sortering: Lineært søk {(binaryTime + sortTime) / linearTime:F2} ganger raskere.";

        
        return timesFaster;
    }
}