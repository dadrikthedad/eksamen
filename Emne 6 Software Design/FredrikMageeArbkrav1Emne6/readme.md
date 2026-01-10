# Fredrik Magee - Emne 6 Mappeinnlevering 1

---

## Handlelisteapplikasjon
Dette er en Blazor WebAssembly-applikasjonen laget for Mappeoppgave 1 i Emne 6.
Handlelisteapplikasjon er en modulær applikasjon med 3 hovedkomponenter: Handleliste, Handlekurv
og Oversikt. Handleliste-komponenten lar brukere legge til varer med navn, mengde, kategori og
estimert pris. Varene vises detaljert i en tabell i komponenten, og varene kan kjøpes eller slettes.
Ved kjøp så flyttes de til vår andre komponent Handlekurv. Her vises en detaljert oversikt over
de kjøpte varene, og bruker har igjen valg om å flytte en vare tilbake til handlelisten eller
slette den. Siste komponent Oversikt viser bruker statistikk og detaljer av innholdet i begge
listene. I tilegg til disse 3 komponentene så er det en UniversitiesList-komponent som er implimentert
for oppgave 2A i eksamen, og videopresentasjonen følger med i zip-filen.


## Steg for å kjøre programmet:

1. Naviger til riktig mappe: FredrikMageeArbkrav1Emne6 eller FredrikMageeArbkrav1Emne6\FredrikMageeArbkrav1Emne6
2. Skriv dotnet run i terminalen
3. Nå kjører programmet på http://localhost:5290 og applikasjonen er klar til å brukes.
4. Det er allerede lagt inn noen varer i handlelisten og handlekurven for å eksperimentere med funksjonene.
5. Trenger du hjelp kan du trykke på ?-knappen øverst i høyre hjørnet

---

## Struktur
Jeg fjernet det meste av komponentene, sidene og en del av CSS-reglene som kom med ferdig malen av WebAssembly.
Vi bruker kun en side, Home.razor, som inneholder alle komponentene.

### Page
Handleliste, Handlekurv og Oversikt er tre av komponentene som vi bruker med DynamicComponent for å vise kun en av
komponentene samtidig. Øverst i headeren la jeg til 3 knapper som styrer hvilke komponent vi ser til enhver tid.
Hver av disse komponentene har hvert sitt fargetema for å tydelig skille mellom dem.

Det er 3 stk lister her som vi henter fra servicen:
1. ShoppingList som er listen til handlelisten
2. CartList som er listen med kjøpte varer
3. CategoryList som er en liste med kategorier

Til denne oppgaven har jeg valgt å bruke to lister for varene, en til handleliste og en til handlekurv. 
Jeg valgte denne løsningen for å tydelig skille mellom listene til hver komponent.
Et alternativ til dette er å ha kun en liste og legge til en bool som forteller om varen er kjøpt eller ikke.
Fordelen med to lister er at vi ikke sender inn objekter inne i komponentene som ikke trenger det, samtidig er ulempen
at det er to lister å holde styr på.

Jeg har valgt å la brukere legge til egne kategorier når de oppretter et nytt element i handlelisten. Neste gang de
legger til en vare så er denne kategorien nå et valg i nedtrekksmenyen. Det gir brukeren mulighet til å lage egne 
kategorier hvis bruker ønsker. Det er ingen mulighet for å slette kategorier for øyeblikket, men det kan komme
i en senere patch. Det er kun aktive kategorier, altså kategorier som har varer i handleliste eller handlekurv, som
dukker opp i statistikk delen. Dette gjør at bruker ikke trenger å se statistikk på tomme kategorier.

ShoppingList og CartList blir sendt nedover hierarkiet med CascadingValue/CascadingParameters, slik at vi ikke 
trenger å sende listene til hver enkelt komponent. Å bruke CascadingParameters er et krav i oppgaven.
CategoryList blir sendt inn i Handleliste-komponenten som et vanlig parameter, da den ikke trengs i andre komponentene.

Home-page mottar EventCallbacks fra barnekomponentene, slik oppgaven spesifiserer. Vi bruker EventFactory til å sikre
at vi oppretter et riktig EventCallback og vi sender det via en ordbok som parameteret til handleliste og
handlekruv komponentene. Metoder i begge disse komponentene så invoker de eventent, og returnerer
referansen til objektet til ønsket metode i servicen. Slik sender vi varer mellom lister og komponenter.

Jeg har valgt å sende referanser til objektet istedenfor å bruke ID. Hadde vi måttet hentet fra en database så måtte 
jeg hatt med et ID-felt eller om vi skulle gjøre en handling/endring på en spesifikk vare. 
For denne oppgaven så klarer vi oss fint uten ID og kun sende referanser.

I headeren har jeg laget en rask logo i Canva og lagret den i mappen images i wwwroot. 

### Service
ShoppingItemService er en service som er laget for å trekke forretningslogikken ut ifra siden/komponentene.
Den er registrert i Program.cs og hentes ut med @inject i Home-siden. I og med at Home er foreldre-komponenten til
alle de andre så trenger vi kun å hente den ut engang herfra og deretter sende nødvendig data nedover komponentene.

I servicen så seeder vi alle listene, og setter de som readonly for å sikre at andre deler av programmet kan
endre referansen på listen. I tilegg setter vi typen som ReadOnlyList for å sikre at ingen andre deler
av programmet kan endre innholdet i listen. Da blir oppretting, endring og sletting kun utført i servicen.

Feil i servicen kjører eventen som trigger StateHasChanged og endrer Home-siden til å vise en feilmelding.
Denne feilmeldingen går det ikke an å fjerne, da det betyr at noe alvorlig har skjeddd, noe som ikke skal skje i det 
hele tatt.

### Models
Modellen ShoppingItem har 5 egenskaper:
- **Name** - string. Minimum 1 til 50 tegn, samt required for å vise at feltet må fylles ut.
- **Amount** - nullable int, default på 1. Må være 0 eller høyere. Satt 1000000 som en tilfeldig maks grense
- **Category** - string. Minimum 1 til 50 tegn
- **EstimatedPrice** - nullable decimal. Må være 0 eller høyere
- **TotalPrice** - nullable decimal, som tar estimert pris * antall varer for å enkelt hente ut total pris

Name står i oppgaveteksten at den er påkrevd. Mengde er valgfritt, men standard på 1. Kategori vurderte jeg som enum,
men jeg valgte å ha det som en string slik at bruker kan lage egne kategorier. Estimert pris er valgfritt, og i og med
at det er penger, så er det anbefalt med decimal for nøyaktighet.

ActiveComponentType er en enum jeg lagde for å kunne skille mellom hvem som er den aktive komponenten og endre
fargene deretter.

### Komponenter
I handleliste-komponenten så har vi en tabell-visning av handlelisten, samt en knapp for å opprette en ny ShoppingItem.
Knappen åpner en modal i komponenten NewItemModal med et skjema bruker kan fylle inn. 
I tabellen er det en knapp for å markere varen som kjøpt, og flytte den til handleliste, og en knapp for å slette varen.
I kolonneoverskriften til kolonnen Kategori er det en nedtrekksmeny som sorterer tabellen etter valgt kategori.

Tabellene i handleliste- og handlekurvkomponentene har scrolling (overflow-y) aktivert hvis det er nok elementer.
I og med at jeg ønsket scroll i bodyen, så klarte jeg ikke å få det fint ved å bruke ```<table>```.
Jeg var nødt til å lage en tabell med grid i app.css (.custom-table). Oversikt-komponenten bruker vanlig ```<table>```.

NewItemModal er en modal med et EditForm-skjema som bruker fyller ut for å legge til en ny vare.
Den har med automatisk validering av attributer som trigges ved trykk på submit-knappen.
Ved trykk på Reset- eller lukk-knappen øverst i høyre hjørne, så resettes verdiene i tabellen.
Klikk på utsiden av Modalen resetter ikke skjemaet i tilfelle brukeren har klikket feil.

Jeg la til en suksessmelding når brukeren legger til en ny vare for å gi brukeren en tilbakemelding i UI-en på at varen
er lagt til, i tilfelle de har scrollet ned i listen og ser ikke at den blir lagt til i UI-en i toppen av listen. 
Denne suksess-meldingen forsvinner automatisk etter 5 sekunder. Ved suksess etter trykk på Submit så lukkes modalen
og det er takket være at vi importerer IJSRuntime og bruker JavaScript til å lukke modalen. 
Jeg vurderte å la den vise at en vare har blitt flyttet fra handleliste til handlekurv, men utelot det fordi vi får
allerede respons i sanntid i listen, ved at varen forsvinner etter at vi har trykket.

I Handlekurv-komponenten så har vi en tabellvisning av handlekurven, samt en knapp for å sende tilbake til handleliste
og en knapp for å slette en vare. 

I Oversikt-komponenten har jeg laget kort til statistikken relevant til listene, og en tabell for å vise
statistikk til hver kategori. Begge listene blir slått sammen for å vise totalen til både varene i handleliste og
varene i handlekurv. Etter kategoriene er gruppert, så sorterer jeg de etter høyeste estimert pris.
Kortet har jeg ekstrahert til en egen komponent, kalt OverviewCard, for å slippe å kopiere kortet 6 ganger.

UserGuideSection er en seksjon i toppen av handleliste, handlekurv og oversikt-komponentene. Den repeterer seg selv
3 ganger, så jeg sparte litt kode ved å ha det i en egen komponent.
Den inneholder en modal, kalt UserGuideModal, som forteller brukeren funksjonaliteten i hver komponent.
Legg til-knappen er også i denne komponenten, men kun i handleliste-komponenten.

### CSS
Alle CSS-reglene jeg har lagt til ligger i app.css under alt det som allerede lå der fra før, under kommentaren
/* ====================== Hovedside ====================== */.



### Andre relevante filer

I _Imports.razor la jeg til alle mappestiene applikasjonen bruker for å slippe å skrive @using i hver fil.

I index.html så la jeg til to linjer:
```
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.1/font/bootstrap-icons.min.css">
```
Denne linjen lar oss bruke Bootstrap Icons for å style knappene med ikoner.

```
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```
Denne linjen lar oss bruke JavaScript inne i en komponent ved å injecte IJSRuntime. Det gir oss mulighet til å 
lukke en modal ved å bruke denne metoden: 
```
await Js.InvokeVoidAsync("eval",
"bootstrap.Modal.getInstance(document.getElementById('newItemModal')).hide()");
```

---

## Eksamens oppgave 2A

I eksamen i Emne 6, Software Design, så valgte jeg oppgave 2A. 
Jeg fant et API som henter universitetene i ett land og jeg valgte å hente ut universitetene i Norge.
Ved å endre siste del av url-en så kan vi spesifisere ønsket land. 

http://universities.hipolabs.com/search?country=Norway

Dermed kunne jeg injecte en HttpClient inne i komponenten og sende en GET-forespørsel til URL-en.
Inneholdet fra forespørselen viser jeg i en tabell slik oppgaven spesifiserer.

Forespørselen gjøres inne i en try-catch for å fange opp feil som kan oppstå, som f.eks. feil URL, 
ingen internett-forbindelse, etc.

Vi bruker await og async for å gjøre asynkrone operasjoner, slik at programmet ikke fryser mens vi venter på
at hentingen skal bli ferdig.

Under hentingen så setter vi en bool som true slik at vi viser "Loading...".

Forespørselen lagres til en liste med University-objekter som jeg har opprettet i Models-mappen.
Der bruker vi attributen [JsonPropertyName("")] for å spesifisere hva hver egenskap skal hente ut.
Noen ganger så fanges det opp automatisk, men ikke alltid, derfor er det lurt å spesifisere det slik.



### Steg for å hente Universiteter

1. Kjør programmet på samme måte som steg 1, 2 og 3 i seksjonen 'Steg for å kjøre programmet'.
2. Øverst i navigasjonsknappene, trykk på Universiteter-knappen helt til høyre.
3. Trykk på Hent universiteter-knappen til venstre.

---

## Spørsmål og svar fra ChatGPT 5.1

Jeg har gitt min ChatGPT en personlig tilpasning / egendefinert instruksjon:
Svar alltid direkte og teknisk, uten introduksjoner, ros, eller uformelle uttrykk.

### Spørsmål 1:

Jeg har laget en egen seksjon med en tabell grid-template-columns: 7rem auto auto auto auto. 
Inne i denne tabellen er det 10 stk div. 5 er header og 5 er inne i foreach-løkke. 
Jeg må gi annen hver foreach stripet effekt slik at det ligner på en vanlig tabell av 
disse 5 divene inne i foreach-løkken. Vi må altså hoppe over 5 celler. Er det en CSS-regel jeg kan bruke?

**AI-svar:**

Bruk nth-child med et mønster som tar hensyn til at hvert “radsett” består av 5 celler.

Eksempel: Hvis de første 5 div-ene er header, og alle etterpå er data, kan du stripe annenhver data-rad slik:

/* Skip header: starter på 6 */
```
.container > div:nth-child(10n + 6),
.container > div:nth-child(10n + 7),
.container > div:nth-child(10n + 8),
.container > div:nth-child(10n + 9),
.container > div:nth-child(10n + 10) {
background: #f5f5f5;
}
```

Forklaring:
– Hver rad = 5 celler.
– Ett stripet sett = 10 celler (5 normal + 5 stripet).
– 10n + X treffer cellene 6–10, 16–20, 26–30 osv.

Tilpass bakgrunn eller andre stilregler etter behov.

### Spørsmål 2:

Hvordan kan jeg lukke en modal i en metode i Blazor? Denne modalen bruker Bootstrap Modal

**AI-svar:**

Bruk IJSRuntime til å kalle Bootstrap-modalens JavaScript-API direkte fra metoden i komponenten.

1. Legg til JS-referanse (om du ikke har det fra før)
   Bootstrap 5 har Modal-klassen globalt tilgjengelig:
```
<script src="bootstrap.bundle.min.js"></script>
```
2. Injiser IJSRuntime i komponenten
@inject IJSRuntime JS
3. Gi modalen et ID
```
<div class="modal fade" id="myModal" tabindex="-1">
    ...
</div>
```
4. Lag en JS-funksjon som lukker modalen

wwwroot/js/site.js:

window.closeModal = (id) => {
const element = document.getElementById(id);
const modal = bootstrap.Modal.getInstance(element)
|| new bootstrap.Modal(element);
modal.hide();
};

Legg til referanse:
```
<script src="js/site.js"></script>
```

5. Kall modalen fra en C#-metode
   private async Task SaveAsync()
   {
   // ... logikk ...
   await JS.InvokeVoidAsync("closeModal", "myModal");
   }

Alternativ uten egen JS-funksjon
Direkte kall:
await JS.InvokeVoidAsync("eval", "bootstrap.Modal.getInstance(document.getElementById('myModal')).hide()");
Dette lukker Bootstrap-modalkomponenten fra en hvilken som helst metode i Blazor.