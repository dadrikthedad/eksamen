using FredrikMageeEmne7Arbkrav.Models;
using Microsoft.EntityFrameworkCore;
namespace FredrikMageeEmne7Arbkrav.Data;

/// <summary>
/// DbContext er en kobling til databasen som er med i EFCore. Den gjør at vi kan utføre operasjoner
/// og hente rader/tabeller fra databasen
/// </summary>
/// <param name="options">I Program.cs så har vi spesifisert hvilken type database det er, og hentet connection
/// stringen fra appsettings, og det henter vi ut her slik at vi slipper å hardkode det hver gang</param>
public class BookDbContext(DbContextOptions<BookDbContext> options) : DbContext(options)
{    
     // Dette er modellen vår og den trengs for å opprette tabellen og kunne hente ut det som er i databasen
     // i metodene våre
     public DbSet<Book> Books { get; set; }
     
     // Vi overrider ModelBuilder-objektet og legger til litt egen logikk. ModelBuilder bygger modellene i databasen
     // og setter regler utifra våre attributer i modellen
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
          // Denne kjører modelBuilderen slik at alle regler og oppsett er gjort før vi legger til vår egne regler
          // eller relasjoner
          base.OnModelCreating(modelBuilder);
          
          // Vi gir ISBN en index da den kanskje brukes ofte for å hente ut bøker, samt vi gjør den unik.
          modelBuilder.Entity<Book>()
               .HasIndex(b => b.ISBN)
               .IsUnique();
     }
}