using FredrikMageeEmne7Arbkrav.Models;

namespace FredrikMageeEmne7Arbkrav.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetBooksAsync(string? title, string? author, int? publicationYear, string? isbn, int? inStock);
    Task<List<Book>> GetBooksInStockAsync();
    Task<Book?> GetBookByIdAsync(int bookId);

    Task<Book> CreateBookAsync(Book book);

    Task SaveChangesAsync();

    Task DeleteBookAsync(Book book);
}