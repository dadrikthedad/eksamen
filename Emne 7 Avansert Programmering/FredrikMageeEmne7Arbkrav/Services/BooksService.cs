using System.ComponentModel.DataAnnotations;
using FredrikMageeEmne7Arbkrav.DTOs.Request.Response;
using FredrikMageeEmne7Arbkrav.DTOs.Requests;
using FredrikMageeEmne7Arbkrav.Extensions;
using FredrikMageeEmne7Arbkrav.Repositories;

namespace FredrikMageeEmne7Arbkrav.Services;

public class BooksService(ILogger<BooksService> logger, IBookRepository bookRepository) : IBooksService
{
    
   /// <summary>
   /// Metode som logger, henter alle bøker via repository utifra tilsendte parameter/filter og mapper fra
   /// Book-entity til BookResponse-objekt
   /// </summary>
   /// <returns>Liste med BookResponser</returns>
    public async Task<List<BookResponse>> GetBooksAsync(
        string? title, 
        string? author, 
        int? publicationYear, 
        string? isbn, int? inStock)
    {
        logger.LogInformation("Getting all books. Payload: {@Payload}", new
        {
            title, 
            author, 
            publicationYear,
            isbn,
            inStock
        });
        
        // Henter ut alle Book-objektene fra databasen
        var books = await bookRepository.GetBooksAsync(title, author, publicationYear, isbn, inStock);
        
        logger.LogInformation("Retrieved {NumberOfBooks} books from the database successfully", books.Count);
        
        // Mapper bøkene om til en DTO BookResponse og sender til kontrolleren
        return books.Select(b => b.ToResponse()).ToList();
    }
    
   /// <summary>
   /// Metoden logger, henter Book-objektene fra repository og mapper Book-objektene til BookResponse-objekter
   /// </summary>
   /// <returns>Liste med BookResponser</returns>
    public async Task<List<BookResponse>> GetBooksInStockAsync()
    {
        logger.LogInformation("Getting all books in stock");
        
        var books = await bookRepository.GetBooksInStockAsync();
        
        logger.LogInformation("Retrieved {NumberOfBooks} books in stock from the database successfully", books.Count);
        
        // Mapper bøkene om til en DTO BookResponse og sender til kontrolleren
        return books.Select(b => b.ToResponse()).ToList();
    }
    
    /// <summary>
    /// Henter ut Book-objektet fra repository og mapper det til et BookResponse-objekt hvis vi fikk et treff
    /// eller returnerer statuskode 404 Not Found
    /// </summary>
    /// <param name="bookId">Boken brukeren vil hente</param>
    /// <returns>En BookResponse hvis vi fant den eller så kaster vi KeyNotFoundException(404 NotFound)</returns>
    public async Task<BookResponse> GetBookByIdAsync(int bookId)
    {
        logger.LogInformation("GetBookById called with id {Id}", bookId);
        // Henter ut Book-objektet eller null
       var book = await bookRepository.GetBookByIdAsync(bookId);
       
       if (book == null)
           throw new KeyNotFoundException($"Book with Id {bookId} not found");
       
       logger.LogInformation("Book {Book} retrieved successfully", book);
       
       // Mapper boken til DTOen BookResponse
       return book.ToResponse();
    }
    
    /// <summary>
    /// Her sender vi DTOen videre til repository for å opprette boken i databasen og vi returnerer et ferdig mappet
    /// Book-objekt tilbake til kontrolleren
    /// </summary>
    /// <param name="request">CreateBookRequest DTO</param>
    /// <returns>Boken som et BookResponse-objekt, eller så kaster EFCore feil for oss hvis noe går galt under 
    /// opprettelse i databasen</returns>
    public async Task<BookResponse> CreateBookAsync(CreateBookRequest request)
    {
        logger.LogInformation("CreateBook called with CreateBookRequest {@Payload}", request); 
        
        // Sender inn et Book-objekt som vi har gjort om fra requesten med ToBook() og oppretter book nå med Id tildelt
        var book = await bookRepository.CreateBookAsync(request.ToBook());
        
        logger.LogInformation("Book created successfully with ID {BookId}", book.Id);
        
        // Mapper Book om til et BookResponse-objekt
        return book.ToResponse();
    }
    
    /// <summary>
    /// Metoden validerer at hvertfall en egenskap skal opppdateres eller så returneres en
    /// ValidationException(400 BadRequest). Deretter hentes boken fra databasen og hvis den ikke
    /// eksisterer returnerer vi 404 Not Found. Eksisterer den så oppdaterer vi egenskapene på boken som har tracking
    /// og lagres til slutt i databasen
    /// </summary>
    /// <param name="bookId">ID-en til boken som skal oppdateres</param>
    /// <param name="request">UpdateBookRequest DTO</param>
    public async Task UpdateBookAsync(int bookId, UpdateBookRequest request)
    {
        // Vi logger om requesten og kan se i loggen om det er med innsendte felter eller ikke
        logger.LogInformation("UpdateBook called with UpdateBookRequest {@Payload}", request); 
        
        // Sjekker at vi har hvertfall en egenskap som skal oppdateres eller så kaster vi en ValidationException
        // som returnerer en 400 BadRequest
        if (request.Title == null && request.Author == null && request.PublicationYear == null &&
            request.ISBN == null && request.InStock == null)
        {
            logger.LogInformation("Book with ID {BookId} cannot be updated without any properties in the request", bookId);
            throw new ValidationException("At least one field is required to updated the book");
        }
        
        // Først sjekker vi om tilsendt Id stemmer med en bok i databasen. Vi henter ut boken eller null
        var book = await bookRepository.GetBookByIdAsync(bookId);
        
        // Er boken null så kaster vi en KeyNotFoundException som returnerer 404 Not Found
        if (book == null)
            throw new KeyNotFoundException($"Book with Id {bookId} not found");
        
        // Vi sjekker hver egenskap i requesten og endrer objektet deretter
        if (!string.IsNullOrWhiteSpace(request.Title))
            book.Title = request.Title.Trim();
        
        if (!string.IsNullOrWhiteSpace(request.Author))
            book.Author = request.Author.Trim();
        
        if (request.PublicationYear != null)
            book.PublicationYear = request.PublicationYear.Value;
        
        if (!string.IsNullOrWhiteSpace(request.ISBN))
            book.ISBN = request.ISBN.Trim();
        
        if (request.InStock != null)
            book.InStock = request.InStock.Value;
        
        // Når vi henter ut boken så har EFCore tracking på den, så endringer som skjer på objektet blir
        // lagret så fremt vi lagrer det med SaveChangesAsync som vi gjør i denne metoden
        await bookRepository.SaveChangesAsync();
        
        logger.LogInformation("Successfully updated book with Id: {Id}", bookId);
    }
        
    /// <summary>
    /// Service metoden som sjekker at boken eksisterer og deretter sletter boken fra databasen
    /// </summary>
    /// <param name="bookId">Boken som skal slettes</param>
    public async Task DeleteBookAsync(int bookId)
    {
        logger.LogInformation("DeleteBook called with {Id}", bookId);
        
        // Først sjekker vi om tilsendt Id stemmer med en bok i databasen. Vi henter ut boken eller null
        var book = await bookRepository.GetBookByIdAsync(bookId);
        
        // Er boken null så kaster vi en KeyNotFoundException
        if (book == null)
            throw new KeyNotFoundException($"Book with Id {bookId} not found");
        
        // Sletter boken med repository-metoden vår
        await bookRepository.DeleteBookAsync(book);
        
        logger.LogInformation("Successfully deleted book with Id: {Id}", bookId);
    }
}