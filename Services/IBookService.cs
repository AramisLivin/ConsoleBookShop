using BookStoreConsole.Models;

namespace BookStoreConsole.Services;

public interface IBookService
{
    IEnumerable<Book> GetBooks(
        string? titleFilter, 
        string? authorFilter, 
        DateTime? dateFilter,
        string? orderByField
    );
        
    void BuyBook(int bookId);
    void Restock(int? bookId, int? count);
}