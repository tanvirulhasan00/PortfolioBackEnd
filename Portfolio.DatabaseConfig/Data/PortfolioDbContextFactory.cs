using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Portfolio.DatabaseConfig.Data
{
    public class PortfolioDbContextFactory : IDesignTimeDbContextFactory<PortfolioDbContext>
    {
        public PortfolioDbContext CreateDbContext(string[] args)
        {
            var currectDirectory = Directory.GetCurrentDirectory();
            var parantDirectory = (Directory.GetParent(currectDirectory)?.FullName) ?? throw new InvalidOperationException("The parent directory could not be determined.");
            var basePath = Path.Combine(parantDirectory, "Portfolio.WebApi");
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

            // Get the connection string
            var connectionString = configuration.GetConnectionString("LocalDatabase");

            // Set up DbContextOptions
            var _optionsBuilder = new DbContextOptionsBuilder<PortfolioDbContext>();
            _optionsBuilder.UseSqlServer(connectionString);

            // Return the context
            return new PortfolioDbContext(_optionsBuilder.Options);
        }
    }
}