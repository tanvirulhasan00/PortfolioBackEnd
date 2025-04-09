using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.PortfolioModels.AuthenticationModels;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.RepositoryConfig.IRepositories.ICustomerMessageRepo;
using Portfolio.RepositoryConfig.IRepositories.IEducationRepo;
using Portfolio.RepositoryConfig.IRepositories.IExperienceRepo;
using Portfolio.RepositoryConfig.IRepositories.IFileRepo;
using Portfolio.RepositoryConfig.IRepositories.IPersonRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectAndTechRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectRepo;
using Portfolio.RepositoryConfig.IRepositories.IServiceRepo;
using Portfolio.RepositoryConfig.IRepositories.ITechnologyRepo;
using Portfolio.RepositoryConfig.IRepositories.IUserRepo;
using Portfolio.RepositoryConfig.Repositories.CustomerMessageRepo;
using Portfolio.RepositoryConfig.Repositories.EducationRepo;
using Portfolio.RepositoryConfig.Repositories.ExperienceRepo;
using Portfolio.RepositoryConfig.Repositories.FileRepo;
using Portfolio.RepositoryConfig.Repositories.PersonRepo;
using Portfolio.RepositoryConfig.Repositories.ProjectAndTechRepo;
using Portfolio.RepositoryConfig.Repositories.ProjectRepo;
using Portfolio.RepositoryConfig.Repositories.ServiceRepo;
using Portfolio.RepositoryConfig.Repositories.UserRepo;

namespace Portfolio.RepositoryConfig.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PortfolioDbContext _dbContext;
        public IPersonRepository Person { get; private set; }
        public IServiceRepository Service { get; private set; }
        public IExperienceRepository Experience { get; private set; }
        public ICustomerMessageRepository CustomerMessage { get; private set; }
        public IEducationRepository Education { get; private set; }
        public IProjectRepository Project { get; private set; }
        public IProjectAndTechnologyRepository ProjectAndTechnology { get; private set; }
        public ITechnologyRepository Technology { get; private set; }
        public IUserRepository Auth { get; private set; }
        public IFileRepository File { get; private set; }
        private readonly string SecretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(PortfolioDbContext dbContext, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            SecretKey = configuration["TokenSetting:SecretKey"] ?? "";
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            File = new FileRepository(_env, _httpContextAccessor);
            Person = new PersonRepository(_dbContext);
            Service = new ServiceRepository(_dbContext);
            Experience = new ExperienceRepository(_dbContext);
            CustomerMessage = new CustomerMessageRepository(_dbContext);
            Education = new EducationRepository(_dbContext);
            Project = new ProjectRepository(_dbContext);
            ProjectAndTechnology = new ProjectAndTechnologyRepository(_dbContext);
            Technology = new TechnologyRepository(_dbContext);
            Auth = new UserRepository(_dbContext, SecretKey, _userManager, _roleManager, _env, _httpContextAccessor);
        }
        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}