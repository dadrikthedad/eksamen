using FredrikMageeEmne7Arbkrav.DTOs.Request.Response;
using FredrikMageeEmne7Arbkrav.DTOs.Requests;

namespace FredrikMageeEmne7Arbkrav.Services;

public interface IBooksService
{
    Task<List<BookResponse>> GetBooksAsync(string? title, string? author, int? publicationYear, string? isbn, int? inStock);
    Task<List<BookResponse>> GetBooksInStockAsync();
    Task<BookResponse> GetBookByIdAsync(int bookId);

    Task<BookResponse> CreateBookAsync(CreateBookRequest request);

    Task UpdateBookAsync(int bookId, UpdateBookRequest request);

    Task DeleteBookAsync(int bookId);

}