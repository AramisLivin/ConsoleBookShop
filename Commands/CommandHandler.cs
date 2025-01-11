using System.Collections.Generic;
using System.Linq;
using BookStoreConsole.Services;

namespace BookStoreConsole.Commands;

public class CommandHandler
{
    private readonly IBookService _bookService;

    public CommandHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public void Handle(string command, Dictionary<string, string> flags)
    {
        switch (command)
        {
            case "get":
                HandleGet(flags);
                break;
            case "buy":
                HandleBuy(flags);
                break;
            case "restock":
                HandleRestock(flags);
                break;
            default:
                Console.WriteLine("Unknown command. Available commands: get, buy, restock");
                break;
        }
    }

    private bool ValidateFlags(Dictionary<string, string> flags, HashSet<string> validFlags)
    {
        var invalidFlags = flags.Keys.Where(flag => !validFlags.Contains(flag)).ToList();
        if (!invalidFlags.Any()) return true;

        Console.WriteLine($"Unknown flags: {string.Join(", ", invalidFlags)}");
        return false;
    }

    private void HandleGet(Dictionary<string, string> flags)
    {
        var validFlags = new HashSet<string> { "title", "author", "date", "order-by" };

        if (!ValidateFlags(flags, validFlags)) return;

        var titleFilter = flags.TryGetValue("title", out var title) ? title : null;
        var authorFilter = flags.TryGetValue("author", out var author) ? author : null;
        var dateFilterString = flags.TryGetValue("date", out var date) ? date : null;
        var orderByField = flags.TryGetValue("order-by", out var order) ? order : null;

        DateTime? dateFilter = null;
        if (!string.IsNullOrWhiteSpace(dateFilterString))
        {
            if (DateTime.TryParse(dateFilterString, out var parsedDate))
            {
                dateFilter = parsedDate;
            }
            else
            {
                Console.WriteLine($"Invalid date format: {dateFilterString}. Use yyyy-MM-dd.");
                return;
            }
        }

        var validOrderFields = new HashSet<string> { "title", "author", "date", "count" };
        if (!string.IsNullOrEmpty(orderByField) && !validOrderFields.Contains(orderByField))
        {
            Console.WriteLine($"Invalid order-by field: {orderByField}. Valid fields: title, author, date, count.");
            return;
        }

        try
        {
            var books = _bookService.GetBooks(titleFilter, authorFilter, dateFilter, orderByField);
            var list = books.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("No books found matching the criteria.");
                return;
            }

            foreach (var book in list)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, " +
                                  $"Year: {book.Year}, Count: {book.Count}");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void HandleBuy(Dictionary<string, string> flags)
    {
        var validFlags = new HashSet<string> { "id" };
        if (!ValidateFlags(flags, validFlags)) return;

        if (!flags.TryGetValue("id", out var value))
        {
            Console.WriteLine("Missing --id flag. Usage: buy --id=<bookId>");
            return;
        }

        if (!int.TryParse(value, out var bookId))
        {
            Console.WriteLine($"Invalid book id: {value}");
            return;
        }

        try
        {
            _bookService.BuyBook(bookId);
            Console.WriteLine($"Book with ID={bookId} was purchased successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void HandleRestock(Dictionary<string, string> flags)
    {
        var validFlags = new HashSet<string> { "id", "count" };
        if (!ValidateFlags(flags, validFlags)) return;

        int? bookId = null;
        int? count = null;

        if (flags.TryGetValue("id", out var idValue))
        {
            if (int.TryParse(idValue, out var parsedId))
            {
                bookId = parsedId;
            }
            else
            {
                Console.WriteLine($"Invalid book id: {idValue}");
                return;
            }
        }

        if (flags.TryGetValue("count", out var countValue))
        {
            if (int.TryParse(countValue, out var parsedCount) && parsedCount > 0)
            {
                count = parsedCount;
            }
            else
            {
                Console.WriteLine($"Invalid count: {countValue}. Count must be a positive number.");
                return;
            }
        }

        try
        {
            if (bookId.HasValue && count.HasValue)
            {
                _bookService.Restock(bookId.Value, count.Value);
            }
            else
            {
                var random = new Random();
                var randomBooks = _bookService.GetBooks(null, null, null, null).ToList();
                foreach (var book in randomBooks.OrderBy(_ => random.Next()).Take(random.Next(1, 5)))
                {
                    var randomCount = random.Next(1, 11);
                    _bookService.Restock(book.Id, randomCount);
                }
            }

            Console.WriteLine("Restock operation completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}