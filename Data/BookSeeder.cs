using BookStoreConsole.Models;

namespace BookStoreConsole.Data;

public static class BookSeeder
{
    public static IEnumerable<Book> GetSeedBooks()
    {
        return new List<Book>
        {
            new()
            {
                Id = 1,
                Author = "Martin Fowler",
                Title = "Refactoring C#",
                Year = 1998,
                Count = 10
            },
            new()
            {
                Id = 2,
                Author = "Eric Evans",
                Title = "Domain-Driven Design",
                Year = 1964,
                Count = 5
            },
            new()
            {
                Id = 3,
                Author = "Jon Skeet",
                Title = "C# In Depth",
                Year = 1999,
                Count = 7
            },
            new()
            {
                Id = 4,
                Author = "Jon Skeet",
                Title = "C# In Depth",
                Year = 2005,
                Count = 7
            },
            new()
            {
                Id = 5,
                Author = "Eric Evans",
                Title = "Domain-Driven Design",
                Year = 1968,
                Count = 5
            },
        };
    }
}