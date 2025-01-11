using BookStoreConsole.Models;

namespace BookStoreConsole.Infrastructure;

public interface IBookRepository
{
    IEnumerable<Book> GetAllBooks();
    Book? GetBookById(int id);
    void UpdateBook(Book book);
}