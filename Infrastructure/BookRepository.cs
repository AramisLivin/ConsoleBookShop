using BookStoreConsole.Data;
using BookStoreConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreConsole.Infrastructure
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContextBase _context;

        public BookRepository(BookContextBase context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.AsNoTracking().ToList();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.Find(id);
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}