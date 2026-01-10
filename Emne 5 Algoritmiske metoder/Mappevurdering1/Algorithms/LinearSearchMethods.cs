namespace Mappevurdering1.Algorithms;

/// <summary>
/// Klassen som inneholder de lineære søke algoritmene
/// </summary>
public static class LinearSearchMethods
{
    /// <summary>
    /// Metode for å gjøre et lineært søk i et int-array, slik spesifisert i oppg1.
    /// Iterer gjennom listen sekvensielt for å finne og returnere indexen til søkemålet.
    /// </summary>
    /// <param name="array">Array vi skal søke igjennom</param>
    /// <param name="target">Søkemålet vi skal prøve å finne</param>
    /// <returns>Returnerer indexen til søkemålet eller -1</returns>
    public static int LinearSearch(int[] array, int target)
    {
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] == target)
                return index;
        }
        return -1;
    }

    /// <summary>
    /// Iterer gjennom en liste sekvensielt for å finne og returnere indexen til søkemålet.
    /// </summary>
    /// <param name="list">Listen vi skal søke igjennom</param>
    /// <param name="target">Søkemålet vi skal prøve å finne</param>
    /// <returns>Returnerer indexen til søkemålet eller -1</returns>
    public static int LinearSearch(List<int> list, int target)
    {
        for (int index = 0; index < list.Count; index++)
        {
            if (list[index] == target)
                return index;
        }
        return -1;
    }

    /// <summary>
    /// Iterer gjennom en LinkedList sekvensielt for å finne og returnere indexen til søkemålet.
    /// </summary>
    /// <param name="list">LinkedList som vi skal søke igjennom</param>
    /// <param name="target">Søkemålet vi skal prøve å finne</param>
    /// <returns>Returnerer indexen til søkemålet eller -1</returns>
    public static int LinearSearch(LinkedList<int> list, int target)
    {
        int index = 0;
        foreach (var number in list)
        {
            if (number == target)
                return index;
            index++;
        }
        return -1;
    }
    
    
}