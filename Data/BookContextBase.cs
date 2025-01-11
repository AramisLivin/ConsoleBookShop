using BookStoreConsole.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreConsole.Data;

public abstract class BookContextBase : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected BookContextBase()
    {
    }

    protected BookContextBase(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Author).HasMaxLength(200);
            entity.Property(b => b.Title).HasMaxLength(300);
        });

        modelBuilder.Entity<Book>().HasData(BookSeeder.GetSeedBooks());
    }
}