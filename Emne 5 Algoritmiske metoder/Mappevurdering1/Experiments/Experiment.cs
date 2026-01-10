using Mappevurdering1.Analysis;
using Mappevurdering1.Models;

namespace Mappevurdering1.Experiments;

/// <summary>
/// Klasse som tar for seg oppg 3 sin undersøkelse og sammenligning. Vi setter opp eksperimentet med forskjellige
/// lister og lagrer det for en analyse til slutt
/// Vi har en metode som setter opp eksperimentet og 2 hjelpemetoder for å dette. 
/// Og vi har en metode som kjører eksperimentene
/// </summary>
public static class Experiment
{
    // ============================================= Felt =============================================
   // For å sikre tilfeldige tall, så initialiserer vi Random kun engang for å redusere sjansen for å få
   // like tall, da den ikke blir opprettet med samme seed ved hver kjøring
   private static readonly Random Random = new();
   
   // ========================== Metoder for å sette opp og kjøre eksperimentene ==========================
   /// <summary>
   /// Setter opp et tilfeldig generert array, søkemål og kjører eksperimentene
   /// 1. Opprett antall størrelser vi skal teste
   /// 2. Kjør eksperimentet 6 ganger
   /// 3. Opprett et array med random tall
   /// 4. Finn et eskisterende søkemål for de 3 første søkene, og ikke eksisterende søkemål for de 3 siste
   /// 5. Kjør eksperimentet 
   /// </summary>
   public static void ArrangeExperiment()
   {
       // Størrelsene på listene vi skal teste
       int[] arraySizes = [1000, 10000, 100000, 1000000, 10000000];
       
       
       // Opprett headeren for resutat-tabellen
       Console.WriteLine(ResultTableBuilder.GetHeader() + "\n" + ConstStrings.Separator);
       
       // Disse to løkkene gir oss 3 kjøringer per størrelse hvor det er sikkert at vi finner et tall, og deretter
       // 3 kjøringer uten å finne et tall, per størrelse i arrayet.
       foreach (var arraySize in arraySizes)
       {
           for (var iteration = 0; iteration < 6; iteration++)
           {
               // Oppretter et array med tilfeldige tall i størrelsen på arraySize
               var array = GetArrayWithRandomNumbers(arraySize);
               // Søkemålet vi skal finne. Finnes i de 3 første iterasjonen, men ikke i de 3 siste
               var target = GetRandomSearchTarget(arraySize, array, iteration < 3);
               // Kjør eksperimentet med arrayan og søkemålet
               RunExperiment(array, iteration < 3, target);
           }

           ResultsAnalyzer.ArrangeAnalyzeResults(arraySize);
       }
   }
   
   /// <summary>
   /// Oppretter en liste og linkedList med like tall som arrayet, kjører deretter eksperimentet
   /// på hver type og lagrer det
   /// </summary>
   /// <param name="array">Array med tilfeldige tall</param>
   /// <param name="isFound">Skal søket finne søkemålet eller ikke</param>
   /// <param name="searchTarget">Søkemålet</param>
   private static void RunExperiment(int[] array, bool isFound, int searchTarget)
   {
       // Oppretter en liste og linkedList som er identiske til arrayet
       var list = array.ToList();
       var linkedList = new LinkedList<int>( array );
       
       // Bruker overloadede metoder for å utføre søk og lagrer resultatet i et SearchResult-objekt
       // RunSearch-metodene har samme navn, men forskjellige parametrer
       SearchResult arrayResult = SearchRunner.RunSearch(array, searchTarget, isFound);
       ResultSaver.SaveResult(arrayResult);
       
       SearchResult listResult = SearchRunner.RunSearch(list, searchTarget, isFound);
       ResultSaver.SaveResult(listResult);
       
       SearchResult linkedListResult = SearchRunner.RunSearch(linkedList, searchTarget, isFound);
       ResultSaver.SaveResult(linkedListResult);
   }  
   
   // ============================================= Hjelpemetoder =============================================
   /// <summary>
   /// Generer et array med tilfeldige tall i ønsket størrelse.
   /// For hver index oppretter vi et tilfeldig tall fra 0 til høyeste tallet i arrayet med arraySize
   /// </summary>
   /// <param name="arraySize">Størrelsen på arrayet</param>
   /// <returns>Array med tilfeldige tall</returns>
   private static int[] GetArrayWithRandomNumbers(int arraySize)
   {
       var array = new int[arraySize];
       
       for (var i = 0; i < arraySize; i++)
       {
           array[i] = Random.Next(0, arraySize);
       }
       
       return array;
   }
   
   /// <summary>
   /// Genererer et søkemål utifra om søkemålet skal finnes eller ikke.
   /// Hvis isFound er true så finner vi et tall fra en tilfeldig index, eller så oppretter vi et tilfeldig tall
   /// høyere enn arrayet
   /// </summary>
   /// <param name="arraySize">Størrelsen på arrayet</param>
   /// <param name="array">Array med tilfeldig genererte tall</param>
   /// <param name="isFound">Boolean som bestemmer om søkemålet skal finnes eller ikke</param>
   /// <returns></returns>
   private static int GetRandomSearchTarget(int arraySize, int[] array, bool isFound) => isFound 
            ? array[Random.Next(0, arraySize)]
            : Random.Next(arraySize, int.MaxValue);
   
}
