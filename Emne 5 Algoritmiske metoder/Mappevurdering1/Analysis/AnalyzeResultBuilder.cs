using Mappevurdering1.Models;
namespace Mappevurdering1.Analysis;

/// <summary>
/// Klassen som bygger og formaterer analyse-oversikten og tabellen som kjører etter eksperimentene er utført.
/// </summary>
public static class AnalyzeResultBuilder
{
    // ========================= Metoder for å opprette tabellene og resultatene oversiktelig =========================
     /// <summary>
    /// Oppretter en liste med strings der hver string er en rad i analyse-tabellen.
    /// Bruker CreateTableRow for riktig formatering
    /// </summary>
    /// <param name="averageResults">Alle gjennomsnittene fra eksperimentene</param>
    /// <returns>Liste med strings</returns>
    public static List<string> CreateAnalyzeTable(AverageResults averageResults)
    {
        // Strings som går igjen mye
        var found = "Ja";
        var notFound = "Nei";

        return
        [
            // Headeren
            CreateTableRow("Datatype", "Funnet", "Avg lineært",
                "Avg binært", "Raskest"),
            ConstStrings.Separator,

            // Array med funnet søkemål
            CreateTableRow(TypeEnum.Array.ToString(), found,
                $"{averageResults.LinearArrayAvg:F6} ms", $"{averageResults.BinaryArrayAvg:F6} ms",
                FastestSearchComparison(averageResults.LinearArrayAvg, averageResults.BinaryArrayAvg)),

            // Array med ikke-funnet søkemål
            CreateTableRow(TypeEnum.Array.ToString(), notFound,
                $"{averageResults.NotFoundLinearArrayAvg:F6} ms",
                $"{averageResults.NotFoundBinaryArrayAvg:F6} ms",
                FastestSearchComparison(averageResults.NotFoundLinearArrayAvg,
                    averageResults.NotFoundBinaryArrayAvg)),

            // List<int> med funnet søkemål
            CreateTableRow(TypeEnum.List.ToString(), found,
                $"{averageResults.LinearListAvg:F6} ms",
                $"{averageResults.BinaryListAvg:F6} ms",
                FastestSearchComparison(averageResults.LinearListAvg, averageResults.BinaryListAvg)),

            // List<int> med ikke-funnet søkemål
            CreateTableRow(TypeEnum.List.ToString(), notFound,
                $"{averageResults.NotFoundLinearListAvg:F6} ms",
                $"{averageResults.NotFoundBinaryListAvg:F6} ms",
                FastestSearchComparison(averageResults.NotFoundLinearListAvg,
                    averageResults.NotFoundBinaryListAvg)),

            // LinkedList<int> med funnet søkemål
            CreateTableRow(TypeEnum.LinkedList.ToString(), found,
                $"{averageResults.LinkedListAvg:F6} ms",
                ConstStrings.Empty, ConstStrings.Empty),

            // LinkedList<int> med ikke-funnet søkemål
            CreateTableRow(TypeEnum.LinkedList.ToString(), notFound,
                $"{averageResults.NotFoundLinkedListAvg:F6} ms",
                ConstStrings.Empty, ConstStrings.Empty)
        ];
    }
    
    /// <summary>
    /// Oppretter de 5 siste linjene med analyse av resultatene til hver array-størrelse
    /// </summary>
    /// <param name="averageResults">Alle gjennomsnittene fra eksperimentene</param>
    /// <returns>Liste med strings</returns>
    public static List<string> CreateResultAnalyze(AverageResults averageResults) =>
    [
        // Gjennomsnittet på array og samligning av lineært mot binært
        $"Gjennomsnittet på sortering av en array er {averageResults.SortArrayAvg:F2} ms." +
        $" Hvis vi regner med sorteringen så er lineært søk raskere " +
        $"med {averageResults.NotFoundBinaryArrayAvg + averageResults.SortArrayAvg
               - averageResults.NotFoundLinearArrayAvg:F6} ms selv når vi itererer igjennom hele arrayen",

        // Gjennomsnittet på List<int> og samligning av lineært mot binært
        $"Gjennomsnittet på sortering av en liste er {averageResults.SortListAvg:F6} ms. " +
        $"Hvis vi regner med sorteringen så er binært søk raskere " +
        $"med {averageResults.NotFoundBinaryListAvg + averageResults.SortListAvg
               - averageResults.NotFoundLinearListAvg:F6} ms selv når vi itererer igjennom hele arrayen",

        // Samligning av sortering på arrays mot List<int>
        $"Sortering av arrays er {averageResults.SortListAvg / averageResults.SortArrayAvg:F6} ganger raskere " +
        $"enn sortering av en liste.",

        // Samennligner array mot List<int>, og List<int> mot LinkedList<int>
        $"Med lineære søk så er array raskere enn lister med " +
        $"{averageResults.NotFoundLinearListAvg - averageResults.NotFoundLinearArrayAvg:F6} ms, " +
        $"og {ListLinkedListComparison(averageResults.NotFoundLinearListAvg,
            averageResults.NotFoundLinkedListAvg)}",
        
        // Sammenligner array mot List<int>
        BinaryArrayListComparison(averageResults.NotFoundBinaryArrayAvg, averageResults.NotFoundBinaryListAvg)
    ];
    
    /// <summary>
    /// Oppretter en rad i tabellen som en string med riktig formatering. For analyse-tabellen
    /// </summary>
    /// <param name="dataType">Array, list eller linkedlist</param>
    /// <param name="found">Funnet/ikke funnet søkemål</param>
    /// <param name="avgLinear">Gjennomsnittet av lineære søk</param>
    /// <param name="avgBinary">Gjennomsnittet av binære søk</param>
    /// <param name="quickest">En setning som omhandler hvem som var raskest og med hvor mye</param>
    /// <returns>En formatert string</returns>
    private static string CreateTableRow(
        string dataType,
        string found,
        string avgLinear,
        string avgBinary,
        string quickest) 
        =>
            $"{dataType, ConstStrings.SmallWidth} " +
            $"| {found, ConstStrings.SmallWidth} " +
            $"| {avgLinear, ConstStrings.BigWidth} " +
            $"| {avgBinary, ConstStrings.BigWidth} " +
            $"| {quickest}";
    
    
    // ====================================== Hjelpemetoder for sammenligning ======================================
    
    /// <summary>
    /// Metoden sjekker hvem som er raskest av linear og binæert søk
    /// </summary>
    /// <param name="linearTime">Millisekunder på lineært søk</param>
    /// <param name="binaryTime">Millisekunder på binært søk</param>
    /// <returns>En formatert string til analyse-delen</returns>
    private static string FastestSearchComparison(double linearTime, double? binaryTime)
    {
        if (linearTime < binaryTime)
            return $"Lineært søk er {binaryTime / linearTime:F2} ganger raskere enn binært søk.";
        
        return $"Binært søk er {linearTime / binaryTime:F2} ganger raskere enn lineært søk.";
    }
    
    /// <summary>
    /// Metoden sjekker hvem datatype som er raskest av liste og linkedList
    /// </summary>
    /// <param name="list">Liste med int</param>
    /// <param name="linkedList">Liste med linkedlist</param>
    /// <returns>En formatert string til analyse-delen</returns>
    private static string ListLinkedListComparison(double list, double linkedList)
    {
        if (list < linkedList)
            return $"List<int> er {linkedList - list:F6} ms raskere enn LinkedList";

        return $"List<int> er {list - linkedList:F6} ms tregere enn LinkedList";
    }
    
    /// <summary>
    /// Metoden sjekker om array er raskere enn list
    /// </summary>
    /// <param name="array">Array med int</param>
    /// <param name="list">List med int</param>
    /// <returns>En formatert string til analyse-delen</returns>
    private static string BinaryArrayListComparison(double? array, double? list)
    {
        if (array < list)
            return $"Med binært søk så er Array raskere enn List<int> med {list  / array:F2} antall ganger.";
        
        return $"Med binært søk så er List<int> raskere enn Array med {array  / list:F2} antall ganger.";
    }
}