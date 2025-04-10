using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.DatabaseConfig.Data
{
    public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        #region A
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion
        #region C
        public DbSet<CustomerMessage> CustomerMessages { get; set; }
        #endregion
        #region E
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        #endregion
        #region P
        public DbSet<Person> Persons { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAndTechnology> ProjectAndTechnologies { get; set; }
        #endregion
        #region S
        public DbSet<Service> Services { get; set; }
        #endregion
        #region T
        public DbSet<Technology> Technologies { get; set; }
        #endregion
    }
}