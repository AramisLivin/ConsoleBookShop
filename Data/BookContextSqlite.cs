using Microsoft.EntityFrameworkCore;

namespace BookStoreConsole.Data
{
    public class BookContextSqlite : BookContextBase
    {
        //можно было бы спрятать
        private const string ConnectionString = "Data Source=bookstore.db";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}