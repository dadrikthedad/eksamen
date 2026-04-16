# ============================== Bygge-stadiet ============================== 
# Image vi bruker for bygging. Inneholder kompilator, Nugget og SDK. Spesifisert sdk som 8.0 da vi bruker .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Oppretter en mappe kalt src for source (kildekode)
WORKDIR /src

# Kopierer alle filene
COPY . .

# Installerer dependencies               
RUN dotnet restore

# Bygger applikasjonen i Release-versjon. -c gir oss valget mellom debug og release, -o velger hvor filene skal lagres
RUN dotnet publish -c Release -o /app/publish

# ============================== Runtime-stadiet ============================== 
# Image for runtime. Mindre image som inneholder .NET runtime for kjøring av ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Oppretter en mappe for applikasjonen
WORKDIR /app

# Exposer på port 8080
EXPOSE 8080

# Kopierer fra build til /publish-mappen hvor den ferdig bygde applikasjonen kjøres ifra
COPY --from=build /app/publish .

# Kjør dotnet ProductAPI.dll på oppstart av containeren
ENTRYPOINT ["dotnet", "ProductAPI.dll"]

