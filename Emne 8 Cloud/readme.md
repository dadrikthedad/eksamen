# Eksamen Emne 8 Cloud-teknikker, web-arkitektur og container teknologi - Fredrik Magee

## Beskrivelse
Vi har laget et enkelt REST-Api med med C# og .NET 8, kalt ProductAPI. Dette API-et er containerisert med Docker,
sammen med en MySQL-database og en Nginx som fungerer som en reverse proxy. API-et sitt image og Nginx sitt image 
er deployet til Docker Hub, og blir brukt i filen docker-compose.yml. Vi har opprettet en EC2 og kjørt docker-compose
i instansen. Deretter migrerte vi til AWS RDS og konfigurerte systemet vårt for CloudWatch.
Oppgavene og tilhørende dokumentasjon til oppgavene er listet opp i denne readme-filen.

## Konfigurasjonsfiler og videor

* SQL-filen, database_setup.sql, ligger i hovedmappen sammen med readme, FredrikMageeEksamenEmne8.
* Oppgave 1 sin docker-compose.yml, og mappene for API-et og nginx, ligger i mappen Oppgave1.
* Oppgave 2 sin docker-compose.yml ligger i mappen Oppgave2, samt bilder av testing i Bilder av testing-mappen.
* Oppgave 3 sin docker-compose.yml og video-filen, Oppgave3-video.mp4, ligger i mappen Oppgave3.
* Oppgave 4 sin docker-compose.yml og video-filen, Oppgave4-video.mp4, ligger i mappen Oppgave4.
* Oppgave 5 sin docker-compose.yml og video-filen, Oppgave5-video.mp4, ligger i mappen Oppgave5. I samme mappe ligger API-et kopiert fra Oppgave1-mappen og oppdatert med koden for imaget vi bruker i Oppgave 5.

## Oppgave 1: Enkel API-løsning med Docker

### Arkitektur og konfigurasjon
#### API
API-et består av 3 endepunkter:
- `GET /api/products` — Henter alle produkter
- `GET /api/products/{id}` — Henter et produkt basert på ID
- `GET /api/health` — Returnerer "API OK"


Eksempel på JSON-respons fra API:
```json
{
  "id": 4,
  "name": "Monitor",
  "brand": "LG",
  "price": 2999,
  "stock": 40
}
```

Installerte dependencies er:
- `dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.2` 
- `dotnet add package Microsoft.EntityFrameworkCore --version 8.0.11`

Pomelo lar oss opprette en tilkobling mot en MySQL-database. Vi bruker EFCore til å lage LINQ-spørringer og 
Pomelo oversetter LINQ-spørringene til SQL-spørringer. EFCore krever en modell for å lage spørringene, så vi har
opprettet Product-modellen i Program.cs. I samme fil registrerer vi Product-tabellen med DbSet i klassen AppDbContext, slik at EFCore vet hvilken tabell spørringene skal brukes mot.

Database-tilkoblingen er konfigurert i appsettings.json: `Server=mysql-db;Port=3306;Database=product_db;User=product_api;Password=securepass`. 
Den blir lest når applikasjonen starter.
Denne stringen lar oss koble opp mot databasen i MySQL-containeren. Oppgaveteksten sier product-db med bindestrek og product_api med underscore, men SQL-scriptet sier product_db og produkt-api på user, så jeg valgte underscore på begge for å gjøre det enkelt.

Oppgaven spesifiserer at API-et må kunne testes uavhengig på port 8080, og jeg er usikker på om det er ment å teste
API-containeren eller API-et kjørt uten container. Jeg har konfigurert i appsettings.json med:
`"Urls": "http://0.0.0.0:8080"`.
Da er ikke default port til API-et 5131 lenger, men 8080. Dette gjør at API-et kan testes nå uavhengig både i 
containeren og kjørt med `dotnet run`.

#### Dockerfile
Det er opprettet en Dockerfile i root-mappen til API-prosjektet for å kjøre API-et i en container.

Dockerfile består av to steg: Første steg for å bygge API-et, og andre steg for å kjøre API-et. Årsaken for at det gjøres i to operasjoner er for å gjøre imagene mindre. Fordeler kan være å sikre raskere nedlastning og mindre diskplass. 

**Steg 1:**
I Dockerfilen så bruker vi Microsoft sitt image som inneholder kompilatoren, NuGet og SDK. Vi bruker .NET 8, samme som API-et.

`FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build`

Vi kopierer alle filene og installerer dependencies med kommandoen:

`RUN dotnet restore`

For å så kunne bygge applikasjonen:

`RUN dotnet publish -c Release -o /app/publish`

**Steg 2:**
Nå som applikasjonen er bygget, så trenger vi ikke kompilatoren, NuGet og SDK lenger, og bruker da et lettere image kun for å kjøre Asp.Net Core-applikasjonen.

`FROM mcr.microsoft.com/dotnet/aspnet:8.0`

Vi eksponerer den på port 8080, og med ENTRYPOINT så bestemmer vi at vi skal kjøre dotnet ProductAPI.dll for å starte applikasjonen:

`ENTRYPOINT ["dotnet", "ProductAPI.dll"]`

#### MySQL Database

I docker-compose.yml så oppretter vi en container for MySql-databasen. Vi velger latest image med latest-taggen og setter opp 
ENV-variablene for å opprette databasen product_db og oppretter en standard bruker for API-et kalt product_api.
Både root-brukeren og product_api-brukeren får passordet securepass. MySql-imaget krever at root-bruker får et passord, og vi bruker admin/root i helsesjekken. Databasen får port 3306 som er standard port for MySql. Vi har en helsesjekk som kjører
hvert 15. sekund og vi bruker denne sjekken til å sikre at databasen er oppe og går før containeren til API-et kjører.

Vi har satt opp persistent lagring av databasen ved å opprette et volum, kalt mysql-db-volume, nederst i compose-filen.
Volumet er koblet til mappen på stien /var/lib/mysql, som er der hvor MySQL lagrer dataen inne i containeren.
Dette sikrer at data blir lagret selv om containerene stoppes/slettes.

Opprettede volumet:
```yml
volumes:
  mysql-db-volume:
```

MySQL-koblet til volumet:
```yml
volumes:
  - mysql-db-volume:/var/lib/mysql
```

I Docker-compose-filen så kopierer vi database_setup.sql inn i mappen /docker-entrypoint-initdb.d/. MySQL kjører alle .sql-filer i denne mappen ved initialisering.
Spørringene i denne filen oppretter da Product-tabellen og brukeren, seeder databasen og gir riktig tilattelser til brukeren.

`- ../database_setup.sql:/docker-entrypoint-initdb.d/init.sql`

Denne SQL-filen er lagret i hovedmappen slik at de forskjellige docker-compose.yml-filene kan referere til samme sql-fil. 
Derfor har vi .. foran /database_setup.sql, siden filen ligger i mappen over.

#### Nginx

I mappen nginx har vi opprettet filen nginx.conf, og med den så konfigurerer vi at http-trafikk som kommer via port 80 (standard port for http) skal sendes til `http://product-api:8080`. `product-api` er navnet på servicen til API-et vi
har satt opp i docker-compose.yml.
I samme mappe har vi en Dockerfile som oppretter en container for nginx. Vi bruker siste utgave av nginx-image med 
latest-taggen og exposer porten på 80.



### Samhandling

Vi kan nå bygge og kjøre docker-compose.yml-filen og det starter 3 tjenester/3 containere som samhandler med hverandre inne i et felles nettverk. 

Nginx lytter og mottar HTTP-trafikk på port 80. Http-forespørsler blir sendt videre til API-et og API-et er koblet
opp til MySQL-databasen. Dermed kan vi hente ut produktene i databasen ved å gå inn via Nginx, altså port 80.


### Testing

Husk å starte Docker-applikasjonen før testing av containerne og docker-compose.yml.

#### API
API-et kan testes uavhengig av database og Nginx ved å kjøre `dotnet run` i riktig sti (FredrikMageEksamenEmne8\Oppgave1\ProductAPI\ProductAPI). API-et blir da tilgjengelig på port 8080, på http://localhost:8080.
Swagger kan brukes på http://localhost:8080/swagger

API-et (containerisert) kan testes med å bygge og deretter kjøre containeren:
1. `docker build -t product-api .`
2. `docker run -p 8080:8080 product-api`

Det er kun helsesjekken som fungerer når API-et kjører uavhengig, da ingen database kjører. 

Curl for å teste API-et (fungerer både ved kjøring lokalt med `dotnet run` eller containerisert):

`curl http://localhost:8080/api/health`

Vi kan starte API-et med docker compose også, uten dependencies, fra riktig sti(FredrikMageEksamenEmne8\Oppgave1):
`docker compose up --no-deps product-api`

#### Docker Compose

1. Naviger til riktig mappe hvor Oppgave1 sin docker-compose.yml-filen ligger. Denne mappen: FredrikMageEksamenEmne8/Oppgave1
2. Filen kan testes med å bygge og deretter kjøre containeren: `docker compose up --build -d`
3. Test med curlene nedenfor, eller besøk http://localhost/swagger for å teste via Nginx, eller besøk http://localhost:8080/swagger for å teste API-et direkte

Endepunktene kan testest direkte med URL-ene (via Nginx):

http://localhost/api/products

http://localhost/api/products/{id}

http://localhost/api/health


Test endepunktene med curl:
Ved å gå via Nginx (port 80):
- `curl http://localhost/api/products`
- `curl http://localhost/api/products/1` - Tallet 1 kan byttes ut med ID-en til de andre produktene
- `curl http://localhost/api/health`

Direkte mot API (port 8080):
- `curl http://localhost:8080/api/products`
- `curl http://localhost:8080/api/products/1`
- `curl http://localhost:8080/api/health`

## Oppgave 2: Publisering av Docker-images til Docker Hub

### Steg

#### Steg 1. Test løsningen lokalt for å sikre at den fungerer som forventet.
Tjenestene er testet lokalt og det fungerer slik som forventet. 
Jeg testet løsningen ved å bruke curl-ene definert i Oppgave 1 Testing-seksjon, både via Nginx på port 80 og mot API-et på port 8080.
I nettleseren min gikk jeg til http://localhost/swagger/index.html og testet alle 3 endepunktene grundig. 

I Oppgave 2-mappen så har jeg lagt ved 9 bilder som viser at jeg navigerer til riktig mappe og kjører docker compose up -d, deretter tester endepunktene.
- Bilde 1 viser at jeg er i riktig sti og kjørt `docker compose up -d`.
- Bilde 2 viser at alt er bygget korrekt og kjører.
- Bilde 3 viser at jeg har navigert til localhost/Swagger/index.html
- Bilde 4-6 viser vellykket testing av endepunktene
- Bilde 7-9 viser testing med direkte URL-er via Nginx

#### Steg 2. Opprett en konto på Docker Hub hvis du ikke allerede har en.
Kontoen min er opprettet første året på Gokstad, da vi hadde emne 2 Databaser.
Min Docker Hub-bruker er gokstadfredrik og repositoriene heter productapi og productapi-nginx.
Lenkene til repository er her:

https://hub.docker.com/repository/docker/gokstadfredrik/productapi/tags

https://hub.docker.com/repository/docker/gokstadfredrik/productapi-nginx/tags

#### Steg 3. Bygg og tag Docker-images for API og Nginx.
Når jeg bygde imagene navigerte jeg inn i mappen til Dockerfile og brukte kommandoene:

`docker buildx build --platform linux/amd64,linux/arm64 -t gokstadfredrik/productapi:latest .`

`docker buildx build --platform linux/amd64,linux/arm64 -t gokstadfredrik/productapi-nginx:latest .`

Vi bygger med multi-architecture ved å bruke `--platform linux/amd64,linux/arm64` slik at den fungerer til maskiner som bruker amd64 og arm64.

#### Steg 4. Push de byggede Docker-images til din private eller offentlige Docker Hub repository.
Deretter pushet jeg de til Docker Hub, med kommandoene:

`docker push gokstadfredrik/productapi:latest`

`docker push gokstadfredrik/productapi-nginx:latest`

#### Steg 5. Lag nye docker-compose.yml slik at den ikke bruker build-kommandoen, men isteden refererer til image-tagene fra Docker Hub.
I mappen Oppgave2 så kopierte jeg inn docker-compose.yml-filen fra Oppgave1-mappen, slik at originalen blir bevart og vi kan bytte ut Build-kommandoene med image fra Docker Hub i yml-filen for oppgave 2.

For API-et så fjerner vi:
```yaml
build:
  context: ./ProductAPI/ProductAPI
  dockerfile: Dockerfile
```

Og erstatter med:
```yaml
image: gokstadfredrik/productapi:latest
```

For nginx så fjerner vi:
```yaml
build:
  context: ./nginx
  dockerfile: Dockerfile
```

Og erstatter med:
```yaml
image: gokstadfredrik/productapi-nginx:latest
```

Til slutt, så kjører jeg `docker compose up -d` fra Oppave2-mappen (FredrikMageEksamenEmne8\Oppgave2) og jeg tester containerene slik jeg gjorde i steg 1 og kan bekrefte at image-ene bygger og fungerer slik forventet.

## Oppgave 3: Deploy API til AWS EC2 med Nginx

I oppgave 3 så oppretter jeg en VPC og en EC2-instanse. Jeg overfører docker-compose.yml og database_setup.sql til instansen, og installerer docker og docker-compose, deretter kjører docker-compose-filen.

Opprettelse av VPC og EC2 vises i videon i mappen til Oppgave3.

For oppgave 3 så kopierte jeg oppgave 2 sin docker-compose.yml og endret stien til database_setup.sql til samme mappe, og ikke mappen over slik vi har i de andre tidligere docker-compose.yml-filene.
Endrer fra:
```yaml
- ../database_setup.sql:/docker-entrypoint-initdb.d/init.sql
```

Og erstatter med:
```yaml
- ./database_setup.sql:/docker-entrypoint-initdb.d/init.sql
```

Deretter bruker jeg disse kommandoene for å overføre begge filene:

`scp -i C:\Users\fredr\Downloads\eksamen-k.pem docker-compose.yml ec2-user@51.20.79.86:~/`

`scp -i C:\Users\fredr\Downloads\eksamen-k.pem ../database_setup.sql ec2-user@51.20.79.86:~/`

Da er begge filene i samme home-mappen i instansen, og docker-compose igjen overfører database_setup.sql til docker-entrypoint-initdb.d-mappen.

Deretter kan jeg navigere inn i instansen med denne kommandoen fra mappen hvor .pem-filen ligger:

`ssh -i "eksamen-key.pem" ec2-user@ec2-51-20-79-86.eu-north-1.compute.amazonaws.com`

Her må jeg installere Docker:

`sudo yum update -y`

`sudo yum install docker -y`

`sudo service docker start`

`sudo usermod -a -G docker ec2-user`

Og deretter Docker-compose:

`sudo curl -L https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose`

`sudo chmod +x /usr/local/bin/docker-compose`

Nå kan vi kjøre containeren:

`docker-compose up -d`

Etter det kan vi teste endepunktene via swagger http://51.20.79.86/swagger/index.html

Eller med curl slik:
`curl http://51.20.79.86/api/products`
`curl http://51.20.79.86/api/products/1`
`curl http://51.20.79.86/api/health`

Endepunktene kan testes direkte mot API-et på port 8080, eks: http://51.20.79.86:8080/swagger/index.html

## Oppgave 4: Migrering til AWS RDS

I oppgave 4 skal vi ikke lenger bruke MySQL-containeren i Docker-compose og opprette en AWS RDS.

I mappen Oppgave4 så kopierer jeg docker-compose.yml fra oppgave 3, og legger til en environment-variabel for API-et.
Denne ENV-en overstyr strengen for databasetilkoblingen som er i appsettings.json. 
ENV-en er feil enn så lenge, vi må bytte ut mysql-db med RDS-endepunktet vi får etter opprettelse.

Lagt til:
```yaml
environment:
  - DatabaseConnection=Server=mysql-db;Port=3306;Database=product_db;User=product_api;Password=securepass
```
Fra samme docker-compose-fil så fjerner vi mysql-db og volumet. Vi fjerner depends on fra API-et. Det er kun API og Nginx igjen i docker-compose-filen.

Vi navigerer til Aurora and RDS i AWS-konsollen og trykker Create Database.

Vi velger:
- Full Configuration - for å kunne endre VPC og opprette en Security Group
- Velger MySQL Engine
- Default i free tier
- DB instance identifier får navnet eksamen-db
- Master username er default admin. SQL-scriptet oppretter API-endepunktet sin bruker
- Master Password og confirm master password er da securepass
- Alt annet er default, så vi blar ned til Connectivity.
- Velger "Dont connect to an EC2 compute resource" i Compute resource
- Velger VPC-en vi opprettet i oppgave 3
- Public access er default på "No" - API-et er det eneste som skal snakke med RDS-en
- Oppretter en ny security group kalt eksamen-rds-sg
- Oppretter en tag med Key = Project og Value = Eksamen
- Create database

Vi må endre tilatt port i Security Group til å kun tillate EC2-en sin security group.
- Vi navigerer til VPC -> Security groups.
- Velger da security group med ID: `sg-05b056b99f2cecba7`
- Velger edit inbound rule
- Sletter den som var der fra før og trykker Add Rule
- Den nye får Type: MySQL/Aurora, Protocol: TCP, Port Range: 3306, Source: Custom - sg-00da7f9969a7d0772 (security group ID til EC2-security groupen)
- Deretter trykker vi 'Save Rules'

Når databasen er ferdig opprettet så har vi fått endepunktet til RDS: `eksamen-db.cv4qoc0mmij3.eu-north-1.rds.amazonaws.com`

Nå endrer vi API-et sin environment-variabel DatabaseConnection til å bruke endepunktet, istedenfor mysql-db-tjenesten vi ikke har lenger:
```yaml
environment:
  - DatabaseConnection=Server=eksamen-db.cv4qoc0mmij3.eu-north-1.rds.amazonaws.com;Port=3306;Database=product_db;User=product_api;Password=securepass
```

Vi overfører denne docker-compose.yml fra Oppgave4-mappen til instansen vår:

`scp -i C:\Users\fredr\Downloads\eksamen-key.pem docker-compose.yml ec2-user@51.20.79.86:~/`

Deretter SSH-er vi inn i instansen og installerer MySQL-klienten for å kunne koble til RDS-en via terminalen:

`sudo yum install mariadb105 -y`

Vi kjører bootstrap-scriptet via admin-brukeren vår, og oppretter databasen, tabellen, brukeren og seeder databasen med 10 produkter:

`mysql -h eksamen-db.cv4qoc0mmij3.eu-north-1.rds.amazonaws.com -u admin -psecurepass < database_setup.sql`

Deretter kjører vi `docker-compose down -v` for å slå av containerne og slette volumet, til slutt `docker-compose up -d` for å starte containerne igjen.

Endepunktene kan nå testes via curl-kommandoene dokumentert i oppgave 3-seksjonen eller Public Ip: http://51.20.79.86/swagger/index.html

## Oppgave 5: AWS CloudWatch Monitorering

I denne oppgaven skal vi installere CloudWatch Agent på EC2-instancen, opprette en rolle med riktige CloudWatch-tillatelser og koble den opp mot EC2-en.
Vi må modifisere API-et til å telle antall API-kall og sende det til CloudWatch. Til slutt skal vi opprette et dashboard og en graf for å se antall API-kall. 

I og med at jeg må endre på API-et for å kunne bruke CloudWatch, og samtidig ønsker å kunne kjøre docker-compose med build i oppgave 1, så kopierer API-et fra Oppgave1 inne i Oppgave5-mappen. Da har vi det originale API-et i Oppgave1-mappen og API-et med CloudWatch-logging i Oppgave5-mapppen.

I det kopierte API-et så laster jeg ned AWS SDK-pakken for CloudWatch:
`dotnet add package AWSSDK.CloudWatch`

Vi må registere AmazonCloudWatch-klienten i DI:
```C#
builder.Services.AddSingleton<IAmazonCloudWatch>(new AmazonCloudWatchClient(Amazon.RegionEndpoint.EUNorth1));
```

Deretter legger vi til et middleware som henter klienten fra DI-en og sender en metric til CloudWatch etter en vellykket http-forespørsel:

```C#
/// <summary>
/// Middleware som sender antall API-kall til CloudWatch
/// </summary>
/// <param name="next">Neste steg i pipelinen</param>
/// <param name="cloudWatch">CloudWatch-klienten</param>
public class CloudWatchApiMiddleware(RequestDelegate next, IAmazonCloudWatch cloudWatch)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Kjører endepunktet ferdig
        await next(context);
        
        // Når endepunktet er kjørt ferdig, så sender vi en metric til CloudWatch
        await cloudWatch.PutMetricDataAsync(new PutMetricDataRequest
        {
            Namespace = "ProductApi",
            MetricData = new List<MetricDatum>
            {
                new()
                { 
                    MetricName = "ApiCallCount",
                    Value = 1,
                    Unit = StandardUnit.Count,
                    Timestamp = DateTime.UtcNow
                }
            }
        });
    }
}
```

Vi registerer den i app-pipelinen.
```C#
app.UseMiddleware<CloudWatchApiMiddleware>();
```

Hver gang et endepunkt blir kalt, så kjører denne metricen etterpå og sender til CloudWatch.
Vi bygger et nytt image, tagger det med "cloudwatch" og pusher til Docker Hub:

`docker buildx build --platform linux/amd64,linux/arm64 -t gokstadfredrik/productapi:cloudwatch .`

`docker push gokstadfredrik/productapi:cloudwatch`

Begge image-ene er her nå:
https://hub.docker.com/repository/docker/gokstadfredrik/productapi/tags

Jeg kopierte docker-compose.yml fra Oppgave4 inn i Oppgave5 og endrer image-et på API-et fra:

```yaml
image: gokstadfredrik/productapi:latest
```

Til:
```yaml
image: gokstadfredrik/productapi:cloudwatch
```

Vi oppretter en role i IAM-panelet. Den får AWS sin ferdige opprettede policy `CloudWatchAgentServerPolicy` som har 
alle nødvendige tillatelser for at EC2-en skal kunne sende metrics til Cloudwatch.
Rollen kaller vi `cloudwatch-ec2-role` og kobler den opp til EC2-instancen via Action-knappen sin Modify IAM-role.

Vi overfører docker-compose-filen til EC2 igjen med kommandoen:

`scp -i C:\Users\fredr\Downloads\eksamen-key.pem docker-compose.yml ec2-user@51.20.79.86:~/`

Jeg SSH-er inn i EC2-instansen og tar ned containerene med:

`docker-compose down`.

Installerer CloudWatch Agent med kommandoene:

`sudo yum install amazon-cloudwatch-agent -y`

Oppretter en config.json-fil med konfigurasjonen for CloudWatch Agent:
```bash
sudo tee /opt/aws/amazon-cloudwatch-agent/bin/config.json << 'EOF'
{
  "agent": {
    "metrics_collection_interval": 60,
    "run_as_user": "root"
  },
  "logs": {
    "logs_collected": {
      "files": {
        "collect_list": [
          {
            "file_path": "/var/lib/docker/containers/*/*.log",
            "log_group_name": "productapi-logs",
            "log_stream_name": "{instance_id}"
          }
        ]
      }
    }
  },
  "metrics": {
    "metrics_collected": {
      "cpu": {
        "measurement": ["cpu_usage_idle", "cpu_usage_user", "cpu_usage_system"]
      },
      "memory": {
        "measurement": ["mem_used_percent"]
      },
      "disk": {
        "measurement": ["used_percent"],
        "resources": ["/"]
      }
    }
  }
}
EOF
```
Dette gir oss systemmetrics for API-et vårt. Det gir oss hvor mye memory vi har brukt, diskplass vi har brukt og metrics relevant til CPU-en.
Vi hentere også loggene fra API-et.

Nå starter vi opp containererne igjen med `docker-compose up -d` for å bruke vårt nye image.

Vi starter CloudWatch Agent:

`sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a fetch-config -m ec2 -s -c file:/opt/aws/amazon-cloudwatch-agent/bin/config.json`

Og vi bekrefter at den fungerer:

`sudo /opt/aws/amazon-cloudwatch-agent/bin/amazon-cloudwatch-agent-ctl -a status`

Vi oppretter et dashboard kalt `eksamen-dashboard` og vi registerer noen widgets for å se at både CloudWatch Agent og vår custom metric fungerer slik oppgaven har spesifisert.
Vi har en line graf for ApiCallCount med Statistic Average og period 5 minutes og vi har en line graf for ApiCallCount med Statistic Sum og period 1 minute.
Vi har en line med mem_used_percent med Statistic Average og period 5 minutes, og noen Line-grafer til.
Vi har et Number-graf for å vise antall API-kall siste 5 minuttene.



