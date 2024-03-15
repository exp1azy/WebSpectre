using Microsoft.EntityFrameworkCore;

namespace WebSpectre.Server.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Agent> Agents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = _config.GetConnectionString("DbConnection");

            optionsBuilder.UseSqlite(connection);
        }
    }
}
