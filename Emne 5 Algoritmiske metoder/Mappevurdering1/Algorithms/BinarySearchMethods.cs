namespace Mappevurdering1.Algorithms;

/// <summary>
/// Klassen som inneholder de binære søke algoritmene
/// </summary>
public static class BinarySearchMethods
{
    /// <summary>
    /// Utfører et binært søk på et array. Halverer delen vi skal søke igjennom ved å finne midten,
    /// sjekke om søkemålet er høyere eller lavere, deretter utføre samme operasjonen helt til vi finner søkemålet
    /// eller til det ikke er flere tall å søke igjennom. Krever sorter array.
    /// Oppgave 2 sitt binære søk
    /// </summary>
    /// <param name="array">Sortert array som vi skal søkes i</param>
    /// <param name="target">Søkemålet</param>
    /// <returns>Indexen til søkemålet eller -1</returns>
    public static int BinarySearch(int[] array, int target)
    {
         // Vi lagrer indeksen helt til venstre i listen, altså 0.
         // Og vi lagrer indeksen helt til høyre, som da er lengden på listen minus en. Minus en siden indeksen
         // starter på 0.
         var left = 0;
         var right = array.Length - 1;
         
         // En while-løkke som itererer helt til venstre indeksen er lavere enn høyere indeksen, og vi har vært
         // igjennom hvert element i listen.
         while (left <= right)
         {
             // For å finne midten, så tar vi venstre indeks og plusser den sammen med summen av høyre indeks minus
             // venstre indeks. Deretter deler vi det på 2.
             var mid = left + (right - left) / 2;
             
             // Returnerer indeksen hvis målet stemmer med elementet i listen med indeksen til mid.
             if (array[mid] == target)
                 return mid;
             
             // Hvis elementet på indeksen er lavere enn målet, så flytter vi da venstre til midten, pluss en.
             // Vi har jo allerede sjekket midten. Det samme gjelder motsatt hvis elementet i
             // indeksen er høyere enn målet, og da flytter vi høyre til midten minus en.
             if (array[mid] < target)
                 left = mid + 1;
             else
                 right = mid - 1;
         }

         return -1;
    }


    /// <summary>
    /// Utfører et binært søk på en liste med int. Halverer delen vi skal søke igjennom ved å finne midten,
    /// sjekke om søkemålet er høyere eller lavere, deretter utføre samme operasjonen helt til vi finner søkemålet
    /// eller til det ikke er flere tall å søke igjennom. Krever sorter array
    /// </summary>
    /// <param name="list">Sortert liste som vi skal søkes i</param>
    /// <param name="target">Søkemålet</param>
    /// <returns>Indexen til søkemålet eller -1</returns>
    public static int BinarySearch(List<int> list, int target)
    {
        // Start- og sluttindexen til listen
        var left = 0;
        var right = list.Count - 1;
        
        // Itererer så lenge det er elementer å søke igjennom
        while (left <= right)
        {
            // Finner midten
            var mid = left + (right - left) / 2;
            
            // SJekker om midten er søkemålet
            if (list[mid] == target)
                return mid;
            
            // Halver delen av listen vi søker i igjen
            if (list[mid] < target)
                left = mid + 1;
            else
                right = mid - 1;
        }
        
        // Søket fant ikke søkemålet
        return -1;
    }
    

}