using BookStoreConsole.Commands;
using BookStoreConsole.Data;
using BookStoreConsole.Infrastructure;
using BookStoreConsole.Services;
using Microsoft.Extensions.Configuration;

namespace BookStoreConsole;

public static class Program
{
    public static void Main(string[] args)
    {
        BookContextBase context = //new PostgresBookContext();
            new BookContextSqlite();

        context.Database.EnsureCreated();
        
        var repo = new BookRepository(context);
        var service = new BookService(repo);
        var handler = new CommandHandler(service);

        Console.WriteLine("BookStore Console App (type 'exit' to quit).");
        Console.WriteLine(
            "Enter commands: get [--title=...] [--author=...] [--year=...] [--order-by=title|author|date|count], buy --id=..., restock [--id=...] [--count=...]");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Trim().ToLower() == "exit") break;
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            try
            {
                var (command, flags) = CommandParser.Parse(parts);
                handler.Handle(command, flags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}