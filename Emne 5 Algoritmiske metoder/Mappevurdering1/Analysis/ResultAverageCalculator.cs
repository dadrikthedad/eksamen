using Mappevurdering1.Models;
namespace Mappevurdering1.Analysis;

/// <summary>
/// Klassen som finner gjennomsnittet av resultatene til eksperimentene
/// </summary>
public static class ResultAverageCaluclator
{
       /// <summary>
    /// Henter gjenommsnittet til hver data type utifra om søkemålet var funnet eller ikke.
    /// Henter og gjennomsnittet for tiden på sortering av liste og array
    /// </summary>
    /// <returns>AverageResults - et objekt med alle gjennomsnittene</returns>
    public static AverageResults GetAveragesFromResults() => new()
    {
        LinearArrayAvg = GetAverage(TypeEnum.Array, true, x => x.LinearTimeMs),
        BinaryArrayAvg = GetAverage(TypeEnum.Array, true, x => x.BinaryTimeMs),
        LinearListAvg = GetAverage(TypeEnum.List, true, x => x.LinearTimeMs),
        BinaryListAvg = GetAverage(TypeEnum.List, true, x => x.BinaryTimeMs),
        LinkedListAvg = GetAverage(TypeEnum.LinkedList, true, x => x.LinearTimeMs),

        NotFoundLinearArrayAvg = GetAverage(TypeEnum.Array, false, x => x.LinearTimeMs),
        NotFoundBinaryArrayAvg = GetAverage(TypeEnum.Array, false, x => x.BinaryTimeMs),
        NotFoundLinearListAvg = GetAverage(TypeEnum.List, false, x => x.LinearTimeMs),
        NotFoundBinaryListAvg = GetAverage(TypeEnum.List, false, x => x.BinaryTimeMs),
        NotFoundLinkedListAvg = GetAverage(TypeEnum.LinkedList, false, x => x.LinearTimeMs),

        SortArrayAvg = GetAverage(TypeEnum.Array),
        SortListAvg = GetAverage(TypeEnum.List)
    };
    
    /// <summary>
    /// Regner ut gjennomsnittet med en LINQ-spørring som filtrerer etter type og funnet/ikke funnet.
    /// Tar og inn en Func slik at vi kan spesifisiere med en labmda hvilke liste vi skal hente ut
    /// </summary>
    /// <param name="dataType">Array, List med int eller LinkedList med int</param>
    /// <param name="isFound">Bool for funnet/ikke funnet søkemål</param>
    /// <param name="selector">Func med SearchResult og double-delegat</param>
    /// <returns>Gjennomsnittet som double</returns>
    private static double GetAverage(TypeEnum dataType, bool isFound, Func<SearchResult, double> selector)
        => ResultsAnalyzer.SearchResults
            .Where(x => x.TypeEnum == dataType && x.IsFound == isFound)
            .Average(selector);
    
    /// <summary>
    /// Regner ut gjennomsnittet til enten sorterings tid for lister eller arrays
    /// </summary>
    /// <param name="dataType">Array eller liste med int</param>
    /// <returns>Gjennomsnittet som double</returns>
    private static double GetAverage(TypeEnum dataType)
        => ResultsAnalyzer.SearchResults
            .Where(x => x.TypeEnum == dataType)
            .Average(x => x.SortTimeMs);
}