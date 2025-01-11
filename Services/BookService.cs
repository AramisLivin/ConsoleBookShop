using BookStoreConsole.Infrastructure;
using BookStoreConsole.Models;

namespace BookStoreConsole.Services;

public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly Random _random = new();

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> GetBooks(
            string? titleFilter, 
            string? authorFilter, 
            DateTime? dateFilter,
            string? orderByField
        )
        {
            var books = _bookRepository.GetAllBooks();

            if (!string.IsNullOrWhiteSpace(titleFilter))
            {
                books = books.Where(b => b.Title.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(authorFilter))
            {
                books = books.Where(b => b.Author.Contains(authorFilter, StringComparison.OrdinalIgnoreCase));
            }
            if (dateFilter.HasValue)
            {
                books = books.Where(b => b.Year == dateFilter.Value.Year);
            }

            books = orderByField?.ToLower() switch
            {
                "title"  => books.OrderBy(b => b.Title),
                "author" => books.OrderBy(b => b.Author),
                "date"   => books.OrderBy(b => b.Year),
                "count"  => books.OrderBy(b => b.Count),
                null     => books.OrderBy(b => b.Id),
                _        => throw new ArgumentException($"Invalid order-by field: {orderByField}")
            };

            return books;
        }


        public void BuyBook(int bookId)
        {
            var book = _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                throw new ArgumentException($"Book with ID {bookId} not found.");
            }

            if (book.Count <= 0)
            {
                throw new InvalidOperationException($"Book with ID {bookId} is out of stock.");
            }

            book.Count--;
            _bookRepository.UpdateBook(book);
        }

        public void Restock(int? bookId, int? count)
        {
            if (bookId.HasValue)
            {
                var book = _bookRepository.GetBookById(bookId.Value);
                if (book == null)
                {
                    throw new ArgumentException($"Book with ID {bookId.Value} not found.");
                }

                if (count.HasValue)
                {
                    if (count.Value <= 0) 
                        throw new ArgumentException("Count must be a positive number.");
                    book.Count += count.Value;
                }
                else
                {
                    var randomAmount = _random.Next(1, 10);
                    book.Count += randomAmount;
                }

                _bookRepository.UpdateBook(book);
            }
            else
            {
                var allBooks = _bookRepository.GetAllBooks().ToList();
                if (!allBooks.Any()) return;

                var howManyBooksToRestock = _random.Next(1, allBooks.Count + 1);
                var booksToRestock = allBooks.OrderBy(_ => _random.Next()).Take(howManyBooksToRestock);

                foreach (var bk in booksToRestock)
                {
                    var randomAmount = _random.Next(1, 10);
                    bk.Count += randomAmount;
                    _bookRepository.UpdateBook(bk);
                }
            }
        }
    }