using Microsoft.EntityFrameworkCore;
using Portfolio.Models.PortfolioModels;

namespace Portfolio.DatabaseConfig.Data
{
    public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
    {
        public DbSet<Person> Persons { get; set; }
    }
}