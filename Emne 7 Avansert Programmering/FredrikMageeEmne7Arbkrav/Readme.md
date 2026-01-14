# Fredrik Magee - Emne 7 Mappeinnlevering 1 

---

## REST Api for en bokhandel

Denne mappeinnleveringen er et REST API for en bokhandel, implementert med C# og ved hjelp av Asp.Net Core WebApi.
Bøkene til bokhandelen lagres i en SQLite-database og endepunktene lar oss utføre CRUD-operasjoner på 
bøkene i denne databasen, altså henting, oppretting, oppdatering og sletting av bøker. 
Med endepunktene til denne API-en kan vi hente alle bøkene, en spesifikk bok utifra bokens ID 
eller hente bøker filtrert utifra egenskapene som f.eks. title, forfatter og utgivelsesår. Vi kan opprette en ny
bok, oppdatere en eksisterende bok utifra bokens ID og slette en bok spesifisert med bokens ID.
Det er implementert et endepunkt som lar brukeren hente alle bøkene som er på lager, for eksamensoppgave A3.
Videopresentasjonen ligger vedlagt i ZIP-filen.

---

## Steg for å kjøre programmet:

1. Naviger til riktig mappe: FredrikMageeEmne7Arbkrav/FredrikMageeEmne7Arbkrav
2. Skriv dotnet run
3. Nå kjøres applikasjonen på http://localhost:5031
4. Test endepunktene slik du måtte ønske. Under er eksempel på hvordan man kan teste med Swagger 
eller medfølgende HTTP-fil.

---

### Swagger
1. Naviger til http://localhost:5031/swagger
2. Fyll inn feltene hvis det trengs og test endepunktene

### Testing med HTTP-filene
I prosjektet her ligger det en HTTP-fil, kalt FredrikMageeEmne7Arbkrav.http, for testing av alle endepunkter.
Den inneholder mange eksempelkall med forskjellige responser og statuskoder. 
Både eksempelkall som er vellykkede, og eksempelkall som feiler.

Det er 2 variabler i denne HTTP-filen:

**baseUrl** som er satt til http://localhost:5031.

**bookId** som er satt til 28. Boken med id 28 er ikke opprettet enda, og da er satt klar for testing av POST, PUT og
           DELETE-endepunktene.

**Slik kan vi teste med .http-filene:**
1. Åpne filen `FredrikMageeEmne7Arbkrav.http`
2. Endre bookId variabelen helt øverst hvis du ønsker eller bruk ID-en som allerede er satt.
3. Alle testene er klar til å kjøres og husk å opprette bok 28 med å teste POST-endepunktet.
4. Ved å bruke den eksisterende ID-en (28) så kan man utføre en test for å opprette, endre og slette en bok uten å
måtte endre variabelen. Ønsker man å teste på en annen bok, så endre variabelen.

   
## Database og Book-entiten:

Databasen er en SQLite-database, opprettet via dotnet ef migrations add CreationOfDatabase og
kolonnene har fått navn og regler via Book-entiteten.

Jeg er usikker på hvordan jeg kan bruke SQL-spørringen til å opprette SQLite databasen, derfor bruker jeg EFCore sin
Code First for å opprette databasen. Det betyr at vi lager modellen først, deretter kjører en migrasjon og EFCore 
oppretter tabellene for oss.

Jeg har satt alle kolonnene som NOT NULL da jeg tolket det slik i oppgaveteksten at alle felt er nødvendig, 
utenom id som er AUTOINCREMENT/SERIAL, altså at den øker med 1 for hver rad.
Bøkene kan ha like forfattere og titler, men de må ha forskjellige ISBN da den er unik til hver bok.
ISBN får også en index slik at det går raskere å søke opp bøker med index, i og med at ISBN blir litt som en Id-kolonne
hvor hver bok har sin egen unike kode.

Id: EFCore vet at en egenskap med navn Id og med typen int er PRIMARY KEY, 
    og setter det automatisk for oss. Denne blir også gitt auto-inkrement, at den øker med 1 hver gang 
    et objekt blir opprettet. 

Title: Maks 200 tegn og kan ikke være null. Sikrer at titlene ikke blir for lange og uhåndterlige.

Author: Maks 100 tegn og kan ikke være null. Sikrer at navnene ikke blir for lange.

PublicationYear: Kan ikke være null og ikke høyere enn 2030. Usikker på om bøker regnes kun som trykte bøker, 
                 håndskrevne bøker eller runer så satt fra år 0 for å være sikker. 2030 er maks verdi, for å slippe
                 å endre det manuelt i databasen. Vi validerer i DTOene at det ikke er høyere enn året 2030.
                 Året 2030 har jeg satt slik at bøker kan legges inn i systemet med tanke på forhåndsbestillinger.

ISBN: Mellom 10 og 25 tegn, unik og kan ikke være null. ISBN er som regel 10-13 siffer, men det kan være flere 
      siffer med tanke på symboler som f.eks. bindestrek. Setter 25 for sikkerhetsskyld.


InStock: InStock kan ikke være null. InStock kan være mindre enn 0 for reserverte bøker. Siden det er int så går den
         ikke høyere enn int sin max verdi som er mye mer enn det denne bokhandelen har plass til på lageret sitt

SQL-filen for Sqlite med tilsvarende oppsett som EFCore lager med modellen vår ligger med i mappen til prosjektet
ved siden av Program.cs.

---

## Filer og struktur:

Vi har en ryddig Program.cs delt opp etter ansvar. Vi har lagt til Serilog for logging, hvilken type database vi har 
med AddDbContext, samt kontrollere, vår globale exception handler, swagger, Book-service og Book-repository.

I appsettings.json har vi lagt til vår connection string og kalt den DatabaseConnection.

Book-modellen til oppgaven ligger i Models-mappen, og består av alle feltene oppgaven ber om.
Vi har en Controller som tar imot HTTP-forespørsler og returnerer responser, en service som inneholder forretningslogikk
og et repository som håndterer CRUD-operasjoner/database-kall med EFCore. 

Forretningslogikk er logikk som gjerne er unikt for hver applikasjon. En bank-applikasjon vil ha forskjellige servicer 
mot en vær-applikasjon, som et eksempel.

Vi fanger opp exceptions og feil med vår GlobalExceptionHandler som ligger i Middleware-mappen.
Det er et middleware vi injiserer inne i DI-en og fanger opp exceptions så fremt vi ikke fanger de opp før med f.eks.
en try-catch (noe vi ikke trenger i denne applikasjonen).
Dette gjør filene våre veldig ryddig, da vi ikke trenger å fange exceptions i hver metode.

Vi bruker også denne GlobalExceptionHandler til å returnere riktig statuskode til brukeren ved å kaste
exceptions som kanskje egentlig ikke er feil, men vanlig forretningslogikk. 
I f.eks. endepunktene hvor ID er nødvendig, så hvis bruker gir en ID som ikke eksisterer så får de 404 Not Found tilbake.
Dette er ikke en feil/exception, men vanlig forretningslogikk. Fortsatt så tenker jeg at å kaste en exception her
ikke gjør noen skade, samt at det holder koden og strukturen ryddig, og vi returnerer da ProblemDetails-objekter
som er en standard måte å returnere feil/exceptions på. Det kan potensielt være upraktisk på veldig
store applikasjoner med mye trafikk da exceptions gjerne har litt mer overhead.

Jeg la til SQLite Error 19 for å fange opp bøker som blir lagt inn med lik ISBN som en allerede eksisterende bok.

I Data-mappen vår så har vi vår BookDbContext-klasse som er en kobling til databasen vår som gjør at vi kan hente ut
og utføre operasjoner på innholdet i databasen. Der presiserer vi hvilke modeller vi har som EFCore skal opprette
tabeller av, samt vi kan lage egne regler til databasen hvis vi trenger det.

Vi har tre DTO (Data Transfer Objects): 
CreateBookRequest og UpdateBookRequest er requestene. 
De brukes for å sende data mellom endepunkt og bruker, og mellom metoder. 
Da slipper vi å skrive hvert eneste felt i parameterne, samt at vi kan
validere de med DataAnnotations.
CreateBookRequest brukes for å opprette en bok i databasen og UpdateBookRequest brukes for å oppdatere egenskapene til
en allerede opprettet bok i databasen.
Begge valideres med attributer med Data Annotations Validations, som er .NETs innebygde validerings system.

BookResponse er response DTO-en vår som vi bruker for å sende Book-modellen vår på en kontrollert måte.
Fordelen med å bruke en DTO istedenfor å sende modellen direkte er det at vi kan bestemme hvilke felter vi skal sende
og ikke sende.

I Extensions-mappen har vi en klasse kalt BookMapperExtension som mapper en CreateBookRequest om til et Book-objekt, og
et Book-objekt om til en BookResponse. Fordelen ved å bruke en extension er at det ser ryddig ut og vi slipper 
å duplisere kode. 

---

## Eksamensoppgave A3

I oppgave 2 så skal vi velge en oppgave for å utvide bokhandel-APIet. A1 hadde jeg allerede implementert fra før,
så da valgte jeg A3. Endepunktet er implementert i kontrolleren, servicen og repository ved siden av de andre
endepunktene/metodene. Endepunktet returnerer en Ok (Statuskode 200) med en liste med BookResponse-objekter, 
eller en tom liste hvis det ikke er noen bøker på lager. Hvis noe galt skjer ved henting fra databasen, så
blir feilen fanget opp med vår GlobalExceptionHandler som igjen returnerer korrekt statuskode.
I repository bruker vi EFCore async LINQ til å hente bøkene fra databasen.

### Eksempelkall

#### GET /api/books/instock - Hent alle bøker med lagerbeholdning høyere enn 0
```http
GET http://localhost:5031/api/books/instock
```

**Respons OK (Statuskode 200) med bøker på lager:**
```json
[
  {
    "id": 1,
    "title": "Ringenes Sverre",
    "author": "Jay Ar Ar Tulkien",
    "publicationYear": 1916,
    "isbn": "RSJRR12328",
    "inStock": 8
  },
  {
    "id": 3,
    "title": "Test the Book",
    "author": "Geir Writerman",
    "publicationYear": 2025,
    "isbn": "BOK123",
    "inStock": 8
  }
]
```

**Respons OK (Statuskode 200) hvis ingen bøker er på lager:**
```json
[]
```

---

## AI-spørsmål til ChatGpt 5:

1. Hvordan kan jeg fange opp en SQLite Error 19 exception i .NET? 
   Hva er syntaxen jeg må ha i ved siden av DbUpdateException i catchen? 
   Ikke vis meg noe annen kode enn kun det vi ser inne i parantesene i en catch

    AI-svar:

   (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)

