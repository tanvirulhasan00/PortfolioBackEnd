using Microsoft.EntityFrameworkCore;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.DatabaseConfig.Data
{
    public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}