using Microsoft.EntityFrameworkCore;

namespace WebSpectre.Client.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = _config.GetConnectionString("SqliteConnection");

            optionsBuilder.UseSqlite(connection);
        }
    }
}
