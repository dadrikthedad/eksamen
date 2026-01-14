using FredrikMageeEmne7Arbkrav.Data;
using FredrikMageeEmne7Arbkrav.Models;
using Microsoft.EntityFrameworkCore;
namespace FredrikMageeEmne7Arbkrav.Repositories;

public class BookRepository(BookDbContext context) : IBookRepository
{
    
    /// <summary>
    /// Denne metoden henter alle Book-objektene fra databasen, eller en tom liste hvis databasen er tom eller bruker
    /// har filtrert bort alle bøkene. Vi bygger en query/spørring med å sjekke om hver parameter er med eller ikke
    /// </summary>
    /// <returns>Liste med Book-objekter eller tom liste</returns>
    public async Task<List<Book>> GetBooksAsync(string? title, string? author, int? publicationYear, string? isbn, int? inStock)
    {
        // AsQueryable() lar oss bygge en LINQ-spørring med det som er i tabellen Books. Vi bruker AsNoTracking()
        // for å skru av tracking. EFCore tracker objektene vi henter ut slik at vi kan gjøre endringer på de og lagre
        // de etterpå. Siden vi skal kun hente bøkene så blir det litt raskere med AsNoTracking() fordi da vet EFCore
        // at vi kun skal hente ut dataen og ikke gjøre noen endringer
        var query = context.Books
            .AsNoTracking()
            .AsQueryable();
        
        // Vi sjekker at hvert felt under er null eller ikke. Vi bruker Contains på parameterne som er string for å
        // sjekke at innsendt string stemmer med noe i kolonnen og ikke trenger å være identisk.
        // Trim() for å fjerne unødvendige mellomrom og ToLower() på både kolonnen i databasen og innsendt parameter
        // for å ikke ta hensyn til små og store bokstaver.
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.ToLower().Contains(title.Trim().ToLower()));
        
        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Author.ToLower().Contains(author.Trim().ToLower()));

        if (publicationYear != null)
            query = query.Where(b => b.PublicationYear == publicationYear);
        
        if (!string.IsNullOrWhiteSpace(isbn))
            query = query.Where(b => b.ISBN.ToLower().Contains(isbn.Trim().ToLower()));
        
        if (inStock != null)
            query = query.Where(b => b.InStock == inStock);
        
        // Kjører LINQ-spørringen til slutt med await og ToListAsync() som lagrer det i en liste.
        return await query.ToListAsync();
    }
    
    /// <summary>
    /// Henter ut alle bøkene med InStock høyere enn 0 fra databasen, uten tracking
    /// </summary>
    /// <returns>Liste med Book-objekter eller tom liste</returns>
    public async Task<List<Book>> GetBooksInStockAsync() =>
        await context.Books
            .AsNoTracking()
            .Where(b => b.InStock > 0).ToListAsync();
    
    /// <summary>
    /// Denne metoden henter en bok fra databasen med FindAsync(). FindAsync() er en rask metode som tar en PRIMARY KEY
    /// som parameter og søker igjennom Idene for å returnere objektet eller null hvis den ikke får noen treff
    /// </summary>
    /// <param name="bookId">Boken bruker vil hente</param>
    /// <returns>Et Book-objekt eller null</returns>
    public async Task<Book?> GetBookByIdAsync(int bookId) => await context.Books.FindAsync(bookId);
    
    
    /// <summary>
    /// Vi legger til boken i databasen og lagrer den
    /// AddAsync() legger boken til databasen, og etter den har kjørt så må vi kjøre SaveChangesAsync() for å lagre det.
    /// Etter vi har kjørt disse to metodene så har nå Book-objektet fått tildelt en Id, og i følge best practice
    /// så returnerer vi Book-objektet tilbake til servicen 
    /// </summary>
    /// <param name="book">Et Book-objekt</param>
    /// <returns>Book-objektet nå med ID-en</returns>
    public async Task<Book> CreateBookAsync(Book book)
    {
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
        return book;
    }
    
    /// <summary>
    /// Denne metoden kjører SaveChangesAsync() asynkront og brukes både når vi oppdaterer en bok.
    /// Det holder ansvaret relatert database til repository
    /// </summary>
    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
    
    
    /// <summary>
    /// Vi sletter boken med å bruke EFCore sin Remove(). Den gjør boken klar for sletting, og blir slettet når vi
    /// lagrer med SaveChangesAsync()
    /// </summary>
    /// <param name="book">Boken som skal slettes</param>
    public async Task DeleteBookAsync(Book book)
    {
        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }
}