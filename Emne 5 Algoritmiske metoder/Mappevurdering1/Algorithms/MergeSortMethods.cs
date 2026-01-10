namespace Mappevurdering1.Algorithms;
/// <summary>
/// Klassen som inneholder sorterings algoritmene - en for array og en for List med int
/// </summary>
public static class MergeSortMethods
{
    // ================================= Metodene som starter sorterings-algoritmene ==================================
    
    /// <summary>
    /// Starter MergeSort for array
    /// </summary>
    /// <param name="array">Arrayet som skal sorteres</param>
    public static void Sort(int[] array)
    {
        MergeSort(array, 0, array.Length - 1);
    }
    
    /// <summary>
    /// Starter MergeSort for List med int
    /// </summary>
    /// <param name="list">Listen som skal sorteres</param>
    public static void Sort(List<int> list)
    {
        MergeSort(list, 0, list.Count - 1);
    }
    
    // ================================= Merge Sort for array ==================================
    
    /// <summary>
    /// Sorterer et array rekursivt. Deler arrayet i 2 på midten og kaller seg selv på hver del igjen,
    /// helt til den ikke kan deles noe mer, og deretter bruker Merge for å sortere og bygge opp hver del til
    /// alle tallene er sortert
    /// </summary>
    /// <param name="array">Arrayet som skal sorteres</param>
    /// <param name="left">Indeksen på tallet i starten av arrayet/delen som skal deles</param>
    /// <param name="right">Indeksen på tallet i slutten av arrayet/delen som skal deles</param>
    private static void MergeSort(int[] array, int left, int right)
    {
        // Hvis left er lik right, så har vi kun ett element igjen og den skal nå sammenslås tilbake igjen
        if (left < right)
        {
            // Vi finner midten
            var middle = left + (right - left) / 2;
            
            // Kaller merge sort igjen rekursivt. Nå skal vi sortere det som er mindre enn midten,
            // inkludert midten
            MergeSort(array, left, middle);
            
            // Vi skal også sortere det over midten, uten å inkludere midten. Derfor må vi legge på +1 på midten
            MergeSort(array, middle + 1, right);
            
            // Deretter merger vi listene tilbake igjen
            Merge(array, left, middle, right);
        }
    }
    
    /// <summary>
    ///  Metode for å sammenslå og sortere delene tilbake til et array. Left er startindeksen på venstresiden,
    /// og middle er både slutten av venstre side og starten av høyre side. Right er siste index av høyre siden
    /// </summary>
    /// <param name="array">Arrayet/delen som skal sammenslås</param>
    /// <param name="left">Startindeksen på arrayet som skal sammenslås</param>
    /// <param name="middle">Midten</param>
    /// <param name="right">Sluttindeksen på arrayet som skal sammenslås</param>
    private static void Merge(int[] array, int left, int middle, int right)
    {
        // Her finner vi antall elementer mellom venstre og midten, og høyre og midten.
        // Elementet i midten skal være i venstre del
        var leftLength = middle - left + 1;
        var rightLength = right - middle;
        
        // Her lager vi nye arrayer med antall elementer som er på venstre siden og høyre siden.
        // Arrays må vite antall elementer på opprettelse
        var leftArray = new int[leftLength];
        var rightArray = new int[rightLength];
        
        // Array.Copy kopierer et array og har 5 parameter. Det første vi sender inn er arrayet vi ønsker å kopiere fra,
        // deretter startindexen vi kopierer ifra, og etter det arrayet vi skal kopiere til. Parameter 4 er start
        // indexen i arrayet vi kopierer til. Siste parameter er antall elementer vi skal legge inn, og
        // det er da lengden på listenpå høyre og venstre side
        Array.Copy(array, left, leftArray, 0, leftLength);
        
        // Vi må ikke få med midten på høyre side, da ville den blitt tatt med dobbelt siden venstre tar den med
        Array.Copy(array, middle + 1, rightArray, 0, rightLength);

        // Peker indexer. De peker på hvor vi er i venstre og høyre arrayet, og hvor vi skal plassere neste element i
        // arrayet vi sorterer
        var leftIndex = 0;
        var rightIndex = 0;
        var mergeIndex = left;
        
        // Denne løkken går så fremt venstre index er lavere enn lengden på venstre array, og det samme gjelder
        // med høyresiden.Det betyr at løkken går så lenge det er tall igjen å sortere.
        // Hvis vi blir ferdig med en side, så tar løkkene under over og sorterer de siste tallene
        while (leftIndex < leftLength && rightIndex < rightLength)
        {
            // Her sjekker vi om venstre array eller høyre array har det minste tallet, og sorterer det laveste tallet
            // først. Det laveste tallet blir satt inn i arrayen vi skal sortere på den indexen vi er på,
            // altså mergeIndex. Slik blir listen sortert, element etter element
            if (leftArray[leftIndex] <= rightArray[rightIndex])
                array[mergeIndex++] = leftArray[leftIndex++];
            else
                array[mergeIndex++] = rightArray[rightIndex++];
        }
        
        // Her sjekker vi om det er noen elementer som ikke er sortert igjen på venstre siden eller høyre siden
        // og plassere dem på sin riktige plass.
        while (leftIndex < leftLength)
        {
            array[mergeIndex++] = leftArray[leftIndex++];
        }

        while (rightIndex < rightLength)
        {
            array[mergeIndex++] = rightArray[rightIndex++];
        }
    }
    
    // ================================= Merge Sort for List<int> ==================================

    /// <summary>
    /// Sorterer en liste rekursivt. Deler listen i 2 på midten og kaller seg selv på hver del igjen,
    /// helt til den ikke kan deles noe mer, og deretter bruker Merge for å sortere og bygge opp hver del til
    /// alle tallene er sortert
    /// </summary>
    /// <param name="list">Listen som skal sorteres</param>
    /// <param name="left">Indeksen på tallet i starten av listen/delen som skal deles</param>
    /// <param name="right">Indeksen på tallet i slutten av listen/delen som skal deles</param>
    private static void MergeSort(List<int> list, int left, int right)
    {
        if (left < right)
        {
            // Vi finner midten
            var middle = left + (right - left) / 2;
            
            // Kaller merge sort igjen rekursivt. Nå skal vi sortere det som er mindre enn midten
            // inkludert midten
            MergeSort(list, left, middle);
            
            // Vi skal også sortere det over midten, uten å inkludere midten. Derfor må vi legge på +1 på midten
            MergeSort(list, middle + 1, right);
            
            // Deretter merger vi listene tilbake igjen
            Merge(list, left, middle, right);
        }
    }

    /// <summary>
    ///  Metode for å sammenslå og sortere delene tilbake til et array
    /// </summary>
    /// <param name="list">Listen/delen som skal sammenslås</param>
    /// <param name="left">Startindeksen på arrayet som skal sammenslås</param>
    /// <param name="middle">Midten</param>
    /// <param name="right">Sluttindeksen på arrayet som skal sammenslås</param>
    private static void Merge(List<int> list, int left, int middle, int right)
    {
        // Antall elementer fra midten til venstre og høyre side
        var leftLength = middle - left + 1;
        var rightLength = right - middle;
        
        // Vi oppretter to lister og vi henter de elementene vi trenger med å bruke getRange()
        // som gir oss alle elementene mellom tallene vi gir i parameterne
        var leftList = new List<int>(list.GetRange(left, leftLength));
        var rightList = new List<int>(list.GetRange(middle + 1, rightLength));
        
        // Indexer som peker hvor vi er i sortering slik at vi vet hva som er ferdig sortert og ikke sortert
        var leftIndex = 0;
        var rightIndex = 0;
        var mergeIndex = left;
        
        // Sorterer tallene i riktig rekkefølge
        while (leftIndex < leftLength && rightIndex < rightLength)
        {
            if (leftList[leftIndex] <= rightList[rightIndex])
                list[mergeIndex++] = leftList[leftIndex++];
            else
                list[mergeIndex++] = rightList[rightIndex++];
        }
        
        // Sorterer de siste elementene hvis det er noen igjen på venstre eller høyre side
        while (leftIndex < leftLength)
        {
            list[mergeIndex++] = leftList[leftIndex++];
        }

        while (rightIndex < rightLength)
        {
            list[mergeIndex++] = rightList[rightIndex++];
        }
    }
}