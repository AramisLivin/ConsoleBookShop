using Microsoft.EntityFrameworkCore;

namespace BookStoreConsole.Data
{
    public class PostgresBookContext : BookContextBase
    {
        //можно было бы спрятать
        private const string ConnectionString = 
            "Host=localhost;Database=bookstore;Username=postgres;Password=postgres";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}