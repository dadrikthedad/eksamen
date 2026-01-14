using FredrikMageeEmne7Arbkrav.DTOs.Request.Response;
using FredrikMageeEmne7Arbkrav.DTOs.Requests;
using FredrikMageeEmne7Arbkrav.Models;
namespace FredrikMageeEmne7Arbkrav.Extensions;

/// <summary>
/// En extensions metode som mapper Book-objekter
/// </summary>
public static class BookMapperExtension
{
    /// <summary>
    /// Mapper Book-objektet til en DTO BookResponse for å sende til frontend
    /// </summary>
    /// <param name="book">Book-objektet vi mapper til en response</param>
    /// <returns>En BookResponse DTO</returns>
    public static BookResponse ToResponse(this Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        Author = book.Author,
        PublicationYear = book.PublicationYear,
        ISBN = book.ISBN,
        InStock = book.InStock
    };
    
    /// <summary>
    /// Mapper en response til et Book-objekt som kan lagres i databasen
    /// </summary>
    /// <param name="book">CreateBookRequest vi mapper om til en Book</param>
    /// <returns>Et Book-objekt</returns>
    public static Book ToBook(this CreateBookRequest book) => new()
    {
        Title = book.Title,
        Author = book.Author,
        PublicationYear = book.PublicationYear,
        ISBN = book.ISBN,
        InStock = book.InStock
    };
}