using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Portfolio.DatabaseConnection.Data;
using System.IO;

namespace Portfolio.DatabaseConnection
{
  public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PortfolioDbContext>
  {
    public PortfolioDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<PortfolioDbContext>();
      // Build the configuration to read the appsettings.json file
      var currentDirectory = Directory.GetCurrentDirectory();
      var parentDirectory = (Directory.GetParent(currentDirectory)?.FullName) ?? throw new InvalidOperationException("The parent directory could not be determined.");
      var basePath = Path.Combine(parentDirectory, "Portfolio.WebApi");
      var configuration = new ConfigurationBuilder()
          .SetBasePath(basePath)  // Make sure to set the correct base path
          .AddJsonFile("appsettings.json")  // Load the appsettings.json file
          .Build();

      // Get the connection string from the appsettings.json
      var connectionString = configuration.GetConnectionString("LocalDatabase");


      // Use the Npgsql provider and pass the connection string
      //optionsBuilder.UseNpgsql(connectionString);
      optionsBuilder.UseSqlServer(connectionString);


      // // Specify the migrations assembly (Portfolio.DatabaseConnection)
      optionsBuilder.UseSqlServer(connectionString, options =>
          options.MigrationsAssembly("Portfolio.DatabaseConnection"));

      return new PortfolioDbContext(optionsBuilder.Options);
    }
  }
}