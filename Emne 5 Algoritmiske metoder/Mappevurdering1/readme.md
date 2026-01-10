# Fredrik Magee - Mappeinnlevering 1 for Eksamen i Emne 5 Algoritmiske metoder

---

## Mappeinnlevering 1 konsollapplikasjon
Dette er en konsollapplikasjon som handler primært om algoritmene lineært og binært søk.
Konsollapplikasjon er laget med C# i Rider.
Konsollapplikasjonen følger kravspesifikasjonene til oppgavene 1, 2 og 3 fra Mappevurderingen 1 og eksamensoppgavene.
Søkene er implementert i applikasjonen og vi utfører tester på søkene og analyser med tidtaker for å måle 
effektiviteten av de to forskjellige søkene med forskjellige datatyper. 
Algoritmen MergeSort() er også med i applikasjonen for å kunne sortere binære søk.
UnitTester for søkene og sorteringen er med i innleveringen, og de finnes i prosjektet TestMappevurdering1.
Videofil ligger med i filen.


---

## Steg for å kjøre applikasjonen:
1. Naviger til riktig mappe: Mappevurdering1 eller Mappevurdering1\Mappevurdering1
2. Skriv i terminalen: dotnet run
3. Nå kjører applikasjonen og bruker kan se applikasjonen i terminalen.
4. Brukeren blir bedt om å taste en tast når brukeren ønsker å kjøre søkene, og for hver analyse til hver størrelse.
5. I ArrangeExperiment-klassen kan du endre på arraySizes. Det å søke i og sortere igjennom 10000000 tall kan være litt
krevende for noen PCer og tar tid. Fjern størrelsene du ikke ønsker å utføre eksperimentet på.

**Hvis du kjører programmet med CTRL + F5 eller Play-knappen 
så klarer ikke applikasjonen å lagre filene i riktig mappe**

## Steg for å kjøre testene:
1. Naviger til mappen TestMappevurdering1. Stien kan da være \Mappevurdering1\TestMappevurdering1
2. Skriv i terminalen: dotnet test
3. Resultatet kommer opp i terminalen med antall vellykkede tester og antall tester som feilet.

---

## Generelt
Jeg valgte Mappeinnlevering 1 fordi jeg mener det går raskere å snakke om og forklare søke 
algoritmene(selv med MergeSort), og koden enn det gjør med Mappeinnlevering 2. 8-10 minutter er, av erfaring, ikke god
nok tid til å snakke om alt en ønsker. Pga sykdom så har jeg vært nødt til å ta mange pauser ved innspilling av video.

Applikasjonen kjører et eksperiment som oppretter et array med tilfeldige tall vi tester søkene og sorteringen på,
med en tidtaker. Dette er da oppgave 3 mappeinnlevering 1.
Arrayet med tilfeldige tall blir opprettet i forskjellige størrelser på 1000, 10 000, 100 000, 1 000 000 og 10 000 000.
Vi tester linært søk på 3 forskjellige datatyper: array, List<int> og LinkedList<int>.
Vi gjør dette for å kunne svare detaljert på refleksjonsdelen i Oppgave 3 relatert til datastrukturvalg.
Derfor har jeg laget 2 ekstra metoder av lineært søk for å kunne utføre søkene på de andre datatypene, og 
en ekstra metode med binært søk og sorteringsmetode for å kunne teste på list itilegg til array.
Binært søk utfører vi kun på array og List<int> da det er mye mer krevende og finne midterste node/verdi av en
LinkedList. Under tidtakingen av binære søk, så tar vi tiden på sorterings metoden for å kunne sammenligne både linært
søk mot binært søk med og uten sorteringen. Resultatet med sortering og uten sortering er det ganske stor forskjell på.

For hver array størrelse så utfører vi 6 forskjellige søk med 6 forskjellige søkemål for å innhente masse data for å 
kunne analysere og sammenligne. 3 av disse søkene har et søkemål som er garantert å finnes, mens 3 av søkemålene 
finnes ikke. Grunnen til det er for å få tiden på søkene når de må søke igjennom hele listen.

Hvert resultate etter et søk er utført blir lagret både til fil og konsoll. Vi får da en oversiktelig tabell i konsollen
med alle resultatene. Etter alle søkene er utført, så finner vi gjennomsnittet på søkene med funnet og ikke funnet
søkemål fordelt på datatype, og gjennomsnittet på sorterings tiden på array og list. Applikasjonen sammenligner
resultatene mot hverandre slik at vi kan se tydelig hvem datatype som det lønner seg å bruke og forkjsellen på søkene.
Dette vises i konsollen til slutt i Analyse-delen.

## Mappevurdering1 Struktur:

Program.cs - Hovedprogrammet som kjører alle testene og analysen

### Algorithms
- BinarySearchMethods - Binært søk for array og lister
- LinearSearchMethods - Lineært søk for array, lister og linkedlist
- MergeSortMethods - Merge sort-metoder for array og lister

### Analysis
- AnalyzeResultBuilder - Klassen som bygger og formaterer analysen og tabellen etter alle søkene er utført.
- FileWriter - Klassen som skriver hvert søkeresultatet til fil
- ResultAverageCalculator - Klassen som regner ut gjennomsnitt av alle søkeresultatene
- ResultsAnalyzer - Klassen som setter opp analyse-delen og kjører den

### Experiments
- Experiment - Klassen som setter opp søkene og kjører søkene
- ResultSaver - Lagrer resultatene til konsoll, fil og en liste med søkeresulateter for analyse-delen
- SearchRunner - Klassen som utfører søkene for arrays, lister og linkedlist
- ResultTableBuilder - Klassen som bygger tabellen for å vise resultatene fra søk

### Models
AverageResults - Klassen som samler alle gjennomsnittene vi har hentet fra liste med SearchResults
ConstStrings - Klasse med stringser som går igjen i flere klasser, samt stiene til logfilene
SearchResults - Samler resultatet fra et søk i et objekt
TypeEnum - Enum for å skille mellom om et søkeresultat er array, list eller linkedlist

### Results 
- allResults.txt - Fil med samlet resultater fra alle datatypene
- arrayResults.txt - Fil med kun array resultatene
- listResults.txt - Fil med kun list resultatene
- linkedListResults.txt - Fil med kun linkedlist resultatene

## TestMappevurdering1 Struktur:
- BinarySearchTests - Tester for binært søk
- LinearSearchTests - Teste for lineært søk

---

Alle klassene, mappene og metodene fra første mappeinnlevering har jeg refaktorert. Jeg syntes det var vanskelig å forstå
koden jeg hadde med første øyekast, så jeg delte opp filene i flere filer og ga de mer spesifikke navn utifra ansvaret
til klassene, slik at det skal være lettere å forstå, samt følge mer korrekt .NET-navnkonvensjon. 
Det har ført til flere filer, men lesligheten har økt.
Metodene er delt opp og omstrukturert til å være mer effektiv og ha mindre ansvar enn det de hadde før. 

## Algoritmer

### Linært søk

Lineært søk går igjennom hvert eneste element sekvensielt til det finner søkemålet eller hele listen er gjennomgått.

Metoden vi har implimentert her iterer igjennom hele arrayet med en for-løkke og sjekker om hvert tall kan være
søkemålet. Returner indexen til søkemålet eller -1 hvis den ikke fant søkemålet.

Et lineært søk har tidskompleksitet på O(1) i best case, og O(n) i worst eller average case. 
For å få O(1) så må elementet være på første index, altså best case. Average case og i worst case så må lineært 
søk søke igjennom hele listen før den finner et element. Det betyr at det jo flere elementer som 
skal søkes i, jo lengre tid vil
det ta. Dette er 0(n) og i en graf vil vi se at tiden det tar vil vokse proporsjonalt med antall elementer. 

```csharp
 public static int LinearSearch(int[] array, int target)
    {
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index] == target)
                return index;
        }
        return -1;
    }
```

### Binært søk

Binært søk finner et søkemål i et arrayet ved å finne midten og sjekke om søkemålet er midten, lavere eller høyere 
enn verdien i midten. Da kan metoden flytte søkeområdet til den delen av arrayet hvor søkemålet skal være.
For hver iterasjon som blir gjort inne i metoden så halveres søkeområdet, helt til den finner søkemålet eller
det ikke er noen elementer å søke igjennom i søkeområde. 
For at binært søk skal fungere så må arrayet vi skal søke i være sortert.

Metoden vi har implementert setter først søkeområdet ved å sette variabelen left og right som første og siste index 
i arrayet. Detter kjører den en while-løkke som først finner midten i hver iterasjon. Den gjør en sjekk om den har
funnet et riktig tall og stopper, eller så sjekker den om søkemålet skal være høyere eller lavere.
Metoden flytter left eller right deretter for å redusere søkeområdet å søke igjennom.
Returner indexen til søkemålet eller -1 hvis den ikke fant søkemålet.

Et binært søk har tidskompleksitet på O(1) i best case, og O(log n) i worst eller average case.
For å få O(1) så må elementet være på indexen i midten på første iterasjon, altså best case. 
I average case og worst case så halveres søkelisten med halvparten i hver iterasjon. O(log n) betyr at selvom størrelsen
på arrayet øker, så vokser søketiden sakte. Et array på 1 000 000 elementer krever ca. 20 sammenligninger,
og sammenligninger mot et array på 1000 elementer tar ca 10 sammenligninger.


```csharp
public static int BinarySearch(int[] array, int target)
    {
         var left = 0;
         var right = array.Length - 1;
   
         while (left <= right)
         {
             var mid = left + ((right - left) / 2);
             
             if (array[mid] == target)
                 return mid;
     
             if (array[mid] < target)
                 left = mid + 1;
             else
                 right = mid - 1;
         }

         return -1;
    }
```

### MergeSort

MergeSort deler et array i to deler ved hver iterasjon. Den kaller seg selv rekursivt og deler hele arrayet helt til
det ikke er mer å dele. Deretter kalles Merge og sammenslår to og to deler som allerede er sortert i 
riktig rekkefølge slik at hele arrayet blir sortert. 

MergeSort sjekker først at det er 2 elementer å sortere med å se om left er lavere enn right. Er det tall å sortere, 
så finner den midten og deler opp delarrayet i to deler og kaller seg selv rekursivt på hver del.
Da vil MergeSort dele opp delarrayene igjen helt til left er lik right(altså ingen flere elementer å sortere),
og begynne å slå sammen delene med Merge.

Merge finner lengdene på delene som skal sammenslås og lagrer det i leftLength og rightLength. 
Deretter kloner vi hver sorterte del inn i hvert sitt array slik at vi ikke overskriver elementer
vi enda ikke har sammenslått i riktig rekkefølge. 
LeftIndex, rightIndex og mergeIndex brukes for å holde styr på hvilke tall som har blitt sammenslått, slik at vi 
har kontroll på de sammenslåtte elementene og elementene som skal sammenslåes.
I første while-løkke så sjekker vi at det er elementer å sammenslå i begge delene, og 
sammenslår i riktig rekkefølge. Det laveste elementet fra venstre eller høyre delarray skal sammenslås først. 
Dermed får vi sammenslått elementene en etter en i riktig rekkefølge. I de to siste while-løkkene så 
sammenslår vi de siste elementene hvis det er elementer igjen i en av de delene.

MergeSort er O(n log n) i best case, average case og worst case. Tidskompleksiteten er lik uansett om listen er 
litt sortert fra før eller ikke. O(n log n) kommer av at for hver gang MergeSort kaller seg selv rekursivt så 
deles arrayet i to, og det gir log n-tidskompleksitet. Merge gjør en lineær sammenslåing av 
hvert nivå så derfor er den n. Dermed blir denne algoritmen O(n log n) i tidskompleksitet. 
Det er grunnen til at jeg valgte MergeSort over Bubble Sort(som har average og worst på O(n2), best på O(n)) 
og Quick Sort(som har average og best på O(n log n) og worst på O(n2). MergeSort har stabil tidskompleksitet 
i motsetning til Bubble og Quick som varierer. Det fører til at resultatene blir ikke påvirket av 
om sorteringsmetoden har best, average eller worst case.

```csharp
    private static void MergeSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            var middle = left + (right - left) / 2;
            MergeSort(array, left, middle);
            
            MergeSort(array, middle + 1, right);
            
            Merge(array, left, middle, right);
        }
    }
        
    private static void Merge(int[] array, int left, int middle, int right)
    {
        var leftLength = middle - left + 1;
        var rightLength = right - middle;

        var leftArray = new int[leftLength];
        var rightArray = new int[rightLength];
        
        Array.Copy(array, left, leftArray, 0, leftLength);
        Array.Copy(array, middle + 1, rightArray, 0, rightLength);

        var leftIndex = 0;
        var rightIndex = 0;
        var mergeIndex = left;
        
        while (leftIndex < leftLength && rightIndex < rightLength)
        {
            if (leftArray[leftIndex] <= rightArray[rightIndex])
                array[mergeIndex++] = leftArray[leftIndex++];
            else
                array[mergeIndex++] = rightArray[rightIndex++];
        }
        
        while (leftIndex < leftLength)
        {
            array[mergeIndex++] = leftArray[leftIndex++];
        }

        while (rightIndex < rightLength)
        {
            array[mergeIndex++] = rightArray[rightIndex++];
        }
    }
```

### Experiments

- Experiment:
Program.cs kjører metoden ArrangeExperiment som oppretter array med tilfeldige tall i forskjellige størrelser
bestemt på forhånd. Størrelsene er 1000, 10000, 100000, 1000000, 10000000.
3 av disse arrayene har et søkemål som eksisterer, og 3 har ikke eksisterende søkemål. Deretter kjører den
eksperimentet med RunExperiment. RunExperiment kjører SearchRunner sine Search-metoder som utfører søket og sortering,
og tar tiden på søkene. Dette blir returnert som et SearchResult-objekt. Deretter blir ResultSaver sin SaveResult
kalt for å lagre resultatene til logg, en liste og fil. 

- Analysis
ResultSaver sin SaveResult kaller først ResultTableBuilder som bygger resultat-tabellen med hjelp av CreateTableRow, som
formatterer stringene slik at det blir en fin og oversiktelig tabell i konsollen.
SaveResult kaller også FileWriter SaveResultToFile som oppretter (hvis ikke alle filene eksisterer fra før),
og lagrer resultatet til fil.

Etter alle søkene er utført så kaller Experiment-klassen ArrangeAnalyzeResults fra klassen ResultsAnalyzer.
Den kaller ResultAvarageCalculator sin metode GetAveragesFromResults som henter gjennomsnittet på egenskapene lagret
i alle SearchResult-objektene våre. Dette sendes til AnalyzeResultBuilder sin CreateAnalyzeTable som bygger
en oversiktelig tabell med dataen, deretter kalles AnalyzeResultBuilder sin CreateResultAnalyze som oppretter de siste
analyse-strengene for analyse-delen i konsollen. Til slutt så kaller Program.cs RunAnalyze for å vise
analyse-tabellen og analyse-oversikten i konsollen.

### Models

Jeg opprettet en model kalt SearchResult for å lagre resultatene fra eksperimentene kjørt for oppgave 3.
Istedenfor å ha mange lister å lagre resultatene i slik det var før, så har vi kun en liste med SearchResult-objekter.
I koden så regner jeg ut gjennomsnittet av veldig mange forskjellige søk, så da fant jeg ut at det var lettere 
å samle de i en klasse som heter AverageResults. TypeEnum er en enum istedenfor å hard kode datatypene som string.
ConstStrings inneholder alle strings som brukes i flere klasser, samt filstiene for å ha det organisert et sted.
Med ConstStrings-klassen og TypeEnum så er vi sikre at vi setter inn riktig verdi. 


## Oppgave 3 Teori del

I Oppgave 3 så genererer jeg mer enn en liste på 1000 elementer, for å kunne virkelig se forskjell på søkene i små lister
og større lister. 

Etter at vi har kjørt igjennom programmet en del ganger og med analysen samt resultatene i filene så kan vi 
trekke noen konklusjoner. Resultatene viser at selv på et "lite" array med kun 1000 elementer, 
så er binære søk raskere med ca 0,001ms, hvis søkemålet ikke finnes. Lineære søkene er raskere hvis målet eksisterer,
ca dobbelt så raskt. Sorteringen av listen er det som er kostbart, og selv på de største listene så blir 
total sortering mindre lønnsomt enn de lineære søkene, så fremt vi kun gjør et søk.
Desto større listene blir, desto mer øker differansen mellom lineære søk mot binære søk. 
Hvis søkemålet ikke eksisterer i listen, så er binært raskest da det slipper å sjekke hvert element.
Analysen viser at på 1 000 000 elementer så kan binære søk være opptil ca. 20000 ganger raskere 
enn lineære søk uten eksisterende søkemål.

Hvis indexen er på de aller første elementene, så kan lineære søk være raskere, da lineært søk kan være O(1).
Vi kan se at lineære søk øker proporsjonalt med størrelsen på listen, at det øker omtrent med 10 gangen
når listen øker med 10 gangen. Det stemmer med O(n), jo større datamengde, jo mer tid vil det ta å 
iterere igjennom det.
Binære søker vokser ikke proporsjonalt, det er kun en liten økning selv om datamengden vokser. 
Det er litt oppstartskostnad, deretter "flater" det seg ut, og det bekrefter at binære søk er O(log n).

Vi kan se på de første søkene at det er litt oppstartskostnad når kompileren kjører, men etter noen få søk så 
stabiliserer det seg. Det går fra ca 0,25 ms til 0,001 ms på 3 søk. Forskjellen på datatyper kommer godt fram. 
Array er lagret ved siden av hverandre, så det går fort å hente fram, og er det raskeste. List<int> er ca dobbelt 
så tregt mot array på lineære søk, og LinkedList er ca dobbelt så tregt av List<int> igjen utifra tallene 
vi kan se i analysen. I teorien så skal array netsten altid være raskere enn List<int>, 
og List<int> skal nesten altid være raskere enn LinkedList<int>, og det kan vi se stemmer i resulatene her.

Vi kan også se at sortering på list er mye tregere enn array og. Årsaken til det kan være MergeSort()-metodene. 
Det virker som at kopieringen av arrayene kan være raskere enn å opprette nye lister hver gang. Quick Sort, og de 
innebygde metodene Sort() og OrderBy() kan være raskere enn min lagde Merge Sort, men Sort() og OrderBy() var ikke 
lov ifølge oppgaveteksten. Quick Sort kan gi varierende resultater da pivotene kan være sentrert noen ganger, 
og andre ganger ikke. Time complexityen på Quick Sort er O(n log n) i beste case og O(n2) i worst case.
Mens Merge Sort er O(n log n) både i best case, average case og worst case, det er mer stabilt.

Binære søk på både List<int> og array er så og si like raske, begge er nede i under 0,002 men array er 
raskere med ca 0,0002ms. Med binære søk på 1000 elementer så er List<int> noen ganger raskere enn array, men i de 
andre analysene så ser vi at den ellers er tregere. Til denne oppgaven har vi brukt søket vi lagde til Oppg 2 for
array og et modifisert søk med utgangspunkt i den metoden for lister. Det finnes innebygde metoder i C# som 
Array.BinarySearch() og List<T>.BinarySerach som kanskje er mer effektiv for sine egne typer og kunne gitt 
oss et annet resultat.

Binære søk brukes i databaser, som PostgresSQL og MySQL. Disse databasene bruker det som kalles et B-Tree.
Et B-Tree er som et oppslagsregister, med nøkler for å finne data raskt. Det kan virke som en telefonbok,
og nøklene kan være feks "A, A" og "M, J" som gjør det lettere å finne navn som "Andreas Andreassen"
eller "Michael Jackson". Nøkkelordene er lagret i noder, og når databasen skal hente en verdi fra en node så brukes
binært søk på nøklene i den noden. Hvis verdien ikke er i den noden, så peker den videre til neste node, 
hvor vi utfører enda et binært søk. Dette pågår helt til vi kommer til den noden hvor ønsket verdi ligger.
Dette gir oss raske søk i databaser.

Kilder:    https://iniakunhuda.medium.com/b-tree-implementation-in-postgresql-deep-dive-into-database-indexing-b1a34032637d
https://en.wikipedia.org/wiki/B-tree
https://www.geeksforgeeks.org/dsa/introduction-of-b-tree-2/

Jeg merket det med de store listene, med 100000000 elementer, at dataen jobbet veldig med å iterere igjennom listene.
Alle datamaskiner er forskjellige, med tanke på programvaren de er bygget opp av, alderen på delene og
innholdet som er lagret på dem. Eksperimentene vil gi forskjellige resultater ut ifra PC-en de blir kjørt på.
Et eksempel kan være at PC-en blir varm under kjøringen av eksperimentene og en vifte øker hastigheten.
Bakgrunnsprosesser kan også forstyrre resultatene under kjøringen. Hvis en app som tar mye ressurser er på i bakgrunn, 
kan det føre til ulike resultater. Kompileren kan også ha en årsak til at det kan være forskjell på resultatet 
fra kjøring til kjøring, og fra PC til PC. Dette er grunner til at det kan være avvik mellom teori og praksis.

Vi kan konkludere med at lineære søk er best på usorterte lister og kan være raskere enn binære søk på lister 
fra 10 000 elementer og lavere. Sorterte lister så lønner binære søk med stor margin, da det reduserer halve 
søkelisten per iterasjon. I databaser med en primary key, indexer og B-Tree så er binære søk mye mer brukt da det 
er bedre for tidsbruk, da det er O(log n) og best for ytelsen og skalarbarhet. Det kan være en fordel å jobbe med 
sorterte lister mot usorterte lister, og hvis det er tilfelle så vil jo binære søk være mye mer brukt.
Binære søk vinner overlegent på sorterte lister, og hvis vi skal søke igjennom samme listen flere ganger eller 
allerede er sortert så bør man bruke binære søk.
Og på grunn av disse grunnene vil jeg spekulere at binære søk er det som er oftest brukt i den digitale verden i dag.


---

## Spørsmål og svar fra ChatGPT 5.1

Jeg har gitt min ChatGPT en personlig tilpasning / egendefinert instruksjon:
Svar alltid direkte og teknisk, uten introduksjoner, ros, eller uformelle uttrykk.

### Spørsmål 1:

Jeg har en metode som tar en liste med et objekt og bruker LINQ og Where() for å filtrere utifra en egenskap til dette
objektet. Hvordan kan jeg legge til en egenskap som et parameter til denne metoden?

**AI-svar:**

Bruk en generisk parameter for egenskapen ved å sende inn en Func<T, TValue> som representerer 
selector-funksjonen for egenskapen du vil filtrere på.

Eksempel:

```csharp
public IEnumerable<T> FilterByProperty<T, TValue>(
IEnumerable<T> source,
Func<T, TValue> propertySelector,
TValue valueToMatch)
{
return source.Where(x => EqualityComparer<TValue>.Default.Equals(propertySelector(x), valueToMatch));
}
```

Bruk:

```csharp
var result = FilterByProperty(myList, x => x.Status, "Active");
```

Dette lar metoden filtrere etter hvilken som helst egenskap uten å hardkode hvilken.

### Spørsmål 2: (Usikker på om dette går som konseptuelt eller ikke)

Hvordan skal man navngi klasser i en konsollapplikasjon? Skal navnet være ansvarsområde deres? Språket er C#

**AI-svar:**
Bruk klassenavn som beskriver den konkrete rollen eller ansvaret klassen har, ikke hva den gjør i øyeblikket. 
I en konsollapplikasjon gjelder de samme prinsippene som ellers i C#:

Beskriv ansvar: CommandParser, FileLoader, UserInputReader, ReportGenerator.

Unngå prosedyrenavn: ikke ParseCommandClass eller DoFileLoad.

Bruk PascalCase: standarden i C#.

En klasse – ett ansvar: hvis navnet blir vanskelig å uttrykke uten “og”, er klassen sannsynligvis for bred.

Domeneorienterte navn når mulig: representer begreper i applikasjonen, f.eks. Invoice, Session, Configuration.

Navngi derfor etter ansvarsområde eller domeneobjekt, ikke etter implementasjonsdetaljer eller brukskontekst 
(som "Console").

**Jeg svarer igjen:**
Navnet bør uttrykke ansvaret: beregne aggregerte verdier basert på en liste av objekter. Eksempler:

StatisticsCalculator

AverageCalculator

MetricsCalculator

ObjectStatisticsService

AggregationService

Hvis klassen kun beregner gjennomsnitt av en spesifikk type objekt, bruk domenenavnet:

CustomerStatisticsCalculator

SensorAverageCalculator

ScoreAggregator

Velg det som mest presist beskriver hva klassen ansvarlig for, ikke hva metoden heter.