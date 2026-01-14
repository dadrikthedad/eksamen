using System.ComponentModel.DataAnnotations;
using FredrikMageeEmne7Arbkrav.DTOs.Requests;
using FredrikMageeEmne7Arbkrav.Services;
using Microsoft.AspNetCore.Mvc;

namespace FredrikMageeEmne7Arbkrav.Controllers;


/// <summary>
/// Book-kontrollern som tar imot HTTP-forespørslene og returnerer responsene
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(IBooksService bookService) : ControllerBase
{
    /// <summary>
    /// Dette endepunktet henter alle bøker, med valgfrie query-parametere slik at bruker kan filtrere
    /// etter alle egenskapene til en Book-klasse, både enkeltvis og flere kombinert samtidig.
    /// I en database med mange bøker kunne det vært aktuelt med paginering.
    /// </summary>
    /// <param name="title">Filtrerer etter tittel</param>
    /// <param name="author">Filtrerer etter forfatter</param>
    /// <param name="year">Filtrerer etter utgivelsesår</param>
    /// <param name="isbn">Filtrerer etter ISBN</param>
    /// <param name="inStock">Filtrerer etter antall på lager</param>
    /// <returns>200 Ok med en liste med BookResponses eller en tom liste</returns>
    [HttpGet]
    public async Task<IActionResult> GetBooks(
        string? title, 
        string? author, 
        int? year,
        string? isbn, 
        int? inStock)
    {
        var books = await bookService.GetBooksAsync(title, author, year, isbn, inStock);
        
        return Ok(books);
    }
    
    /// <summary>
    /// Endepunkt som returnerer alle bøkene som finnes på lager
    /// </summary>
    /// <returns>200 Ok med en liste med BookResponses eller en tom liste</returns>
    [HttpGet("instock")]
    public async Task<IActionResult> GetBooksInStock()
    {
        var books = await bookService.GetBooksInStockAsync();
        
        return Ok(books);
    }
    
    /// <summary>
    /// Dette endepunktet returnerer en bok utifra tilsendt Id. Vi validerer Id-en
    /// med DataAnnotations og at det er en gyldig verdi.
    /// </summary>
    /// <param name="bookId">ID-en til boken bruker ønsker å hente</param>
    /// <returns>StatusCode 200 med boken som en BookResponse DTO eller 404 Not Found hvis boken ikke eksisterer.
    /// BadRequest hvis attributene ikke stemmer</returns>
    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetBookById(
        [FromRoute]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be higher than 0 and a valid number")]
        int bookId)
    {
        // Henter ut en BookResponse DTO eller null med innsendt Id
        var book = await bookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }
    
    
    
    /// <summary>
    /// Dette endepunktet tar inn en DTO (Data Transfer Object) med verdiene vi skal bruke for å opprette en book.
    /// Vi validerer feltene med attributer i CreateBookRequest
    /// </summary>
    /// <param name="request">CreateBookRequest DTO</param>
    /// <returns>201 Created med relevant data eller BadRequest hvis noen av attributene ikke stemmer</returns>
    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookRequest request)
    {
        // Sender DTOen videre til servicen og får en BookResponse objekt hvis alt gikk bra
        var book = await bookService.CreateBookAsync(request);
        
        // Vi returnerer stien til å hente ut boken igjen, som er da endepunktet GetBookById og vi returnerer ID-en for 
        // å hente den igjen. Vi returnerer også Book-objektet tilbake til brukeren
        return CreatedAtAction(nameof(GetBookById), new { bookId = book.Id}, book);
    }
    
    /// <summary>
    /// Dette endepunktet oppdaterer en eksisterende bok Vi tar imot en UpdateBookRequest DTO med nullable
    /// felter slik at vi vi kan oppdatere kun de feltene vi ønsker
    /// </summary>
    /// <param name="bookId">Boken bruker vil oppdatere</param>
    /// <param name="request">UpdateBookRequest DTO</param>
    /// <returns>204 No Content hvis vellykket oppdatering, 400 BadRequest på attributer som er feil eller
    /// 404 Not Found hvis ID-en ikke stemmer</returns>
    [HttpPut("{bookId}")]
    public async Task<IActionResult> UpdateBook(
        [FromRoute]
        [Range(1, int.MaxValue , ErrorMessage = "ID must be valid number higher than 0")]
        int bookId, 
        UpdateBookRequest request)
    {
        // Vi oppdaterer boken med service-metoden
        await bookService.UpdateBookAsync(bookId, request);
        
        // No Content brukes hvis frontend ikke trenger noe annen data etter en endring. Returnerer kun 204 No Content
        // som sier til frontend at dette var en suksess
        return NoContent();
    }
    
    /// <summary>
    /// Vi sletter en bok med tilsendt ID. Validerer med attributene at vi har fått en ID og at det er et gyldig tall
    /// </summary>
    /// <param name="bookId">Boken som skal slettes</param>
    /// <returns>204 No Content, 400 BadRequest ved feil validering på Id-en
    /// eller NotFound 404 hvis Id-en ikke finnes</returns>
    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBook(
        [FromRoute] 
        [Range(1, int.MaxValue, ErrorMessage = "ID must be higher than 0 and a valid number")]
        int bookId)
    {
        // Kaller service-metoden og prøver å slette boken
        await bookService.DeleteBookAsync(bookId);
        return NoContent();
    }
    
    
}