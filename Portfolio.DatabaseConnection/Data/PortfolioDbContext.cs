using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models.DbModels;

namespace Portfolio.DatabaseConnection.Data
{
    public class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
    {
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
        #region L
        public DbSet<LocalUser> LocalUsers { get; set; }
        #endregion
        #region S
        public DbSet<Service> Services { get; set; }
        #endregion
        #region T
        public DbSet<Technology> Technologies { get; set; }
        #endregion
    }
}