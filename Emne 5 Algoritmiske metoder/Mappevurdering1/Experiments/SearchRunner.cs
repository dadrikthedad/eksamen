using System.Diagnostics;
using Mappevurdering1.Algorithms;
using Mappevurdering1.Models;
namespace Mappevurdering1.Experiments;

/// <summary>
/// Klassen som tar tiden på de lineareog binære søkene, samt sortering, i listene satt opp i Experiments-klassen
/// </summary>
public static class SearchRunner
{
    private static readonly Stopwatch Stopwatch = new();

    /// <summary>
    /// Utfører lineart søk, sortering og binært søk på et array
    /// </summary>
    /// <param name="array">int[]</param>
    /// <param name="searchTarget">Søkemålet søket skal finne</param>
    /// <param name="isFound">Bool sendes inn her for å lagre ved opprettelse</param>
    /// <returns>SearchResult-objekt med array størrelse, resultet fra Stopwatch og isFound-bool lagret</returns>
    public static SearchResult RunSearch(int[] array, int searchTarget, bool isFound)
    {
        SearchResult result = new()
        {
            ArraySize = array.Length,
            TypeEnum = TypeEnum.Array,
            IsFound = isFound
        };
        
        Stopwatch.Restart();
        result.LinearResult = LinearSearchMethods.LinearSearch(array, searchTarget);
        Stopwatch.Stop();
        result.LinearTimeMs = Stopwatch.Elapsed.TotalMilliseconds;
            
        Stopwatch.Restart();
        MergeSortMethods.Sort(array);
        Stopwatch.Stop();
        result.SortTimeMs = Stopwatch.Elapsed.TotalMilliseconds;
            
        Stopwatch.Restart();
        result.BinaryResult = BinarySearchMethods.BinarySearch(array, searchTarget);
        Stopwatch.Stop();
        result.BinaryTimeMs = Stopwatch.Elapsed.TotalMilliseconds;

        return result;
    }

    /// <summary>
    /// Utfører lineart søk, sortering og binært søk på en liste
    /// </summary>
    /// <param name="list">List med int</param>
    /// <param name="searchTarget">Søkemålet søket skal finne</param>
    /// <param name="isFound">Bool sendes inn her for å lagre ved opprettelse</param>
    /// <returns>SearchResult-objekt med array størrelse, resultet fra Stopwatch og isFound-bool lagret</returns>
    public static SearchResult RunSearch(List<int> list, int searchTarget, bool isFound)
    {
        SearchResult result = new()
        {
            ArraySize = list.Count,
            TypeEnum = TypeEnum.List,
            IsFound = isFound
        };

        Stopwatch.Restart();
        result.LinearResult = LinearSearchMethods.LinearSearch(list, searchTarget);
        Stopwatch.Stop();
        result.LinearTimeMs = Stopwatch.Elapsed.TotalMilliseconds;
            
        Stopwatch.Restart();
        MergeSortMethods.Sort(list);
        Stopwatch.Stop();
        result.SortTimeMs = Stopwatch.Elapsed.TotalMilliseconds;
            
        Stopwatch.Restart();
        result.BinaryResult = BinarySearchMethods.BinarySearch(list, searchTarget);
        Stopwatch.Stop();
        result.BinaryTimeMs = Stopwatch.Elapsed.TotalMilliseconds;

        return result;
    }

    /// <summary>
    /// Utfører lineart søk, sortering og binært søk på en liste
    /// </summary>
    /// <param name="linkedList">LinkedList med int</param>
    /// <param name="searchTarget">Søkemålet søket skal finne</param>
    /// <param name="isFound">Bool sendes inn her for å lagre ved opprettelse</param>
    /// <returns>SearchResult-objekt med array størrelse, resultet fra Stopwatch og isFound-bool lagret</returns>
    public static SearchResult RunSearch(LinkedList<int> linkedList, int searchTarget, bool isFound)
    {
        SearchResult result = new()
        {
            ArraySize = linkedList.Count,
            TypeEnum = TypeEnum.LinkedList,
            IsFound = isFound
        };
            
        Stopwatch.Restart();
        result.LinearResult = LinearSearchMethods.LinearSearch(linkedList, searchTarget);
        Stopwatch.Stop();
        result.LinearTimeMs = Stopwatch.Elapsed.TotalMilliseconds;
        
        return result;
    }
}