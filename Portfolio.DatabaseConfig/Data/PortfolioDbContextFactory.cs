using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;



namespace Portfolio.DatabaseConfig.Data
{
    public class PortfolioDbContextFactory : IDesignTimeDbContextFactory<PortfolioDbContext>
    {
        public PortfolioDbContext CreateDbContext(string[] args)
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
                var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Portfolio.WebApi");
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
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