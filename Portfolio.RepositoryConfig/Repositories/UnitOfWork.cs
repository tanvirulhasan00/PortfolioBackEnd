using Portfolio.DatabaseConnection.Data;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.RepositoryConfig.IRepositories.ICustomerMessageRepo;
using Portfolio.RepositoryConfig.IRepositories.IEducationRepo;
using Portfolio.RepositoryConfig.IRepositories.IExperienceRepo;
using Portfolio.RepositoryConfig.IRepositories.IPersonRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectAndTechRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectRepo;
using Portfolio.RepositoryConfig.IRepositories.IServiceRepo;
using Portfolio.RepositoryConfig.IRepositories.ITechnologyRepo;
using Portfolio.RepositoryConfig.IRepositories.IUserRepo;
using Portfolio.RepositoryConfig.Repositories.CustomerMessageRepo;
using Portfolio.RepositoryConfig.Repositories.EducationRepo;
using Portfolio.RepositoryConfig.Repositories.ExperienceRepo;
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
        public IUserRepository LocalUser { get; private set; }
        public UnitOfWork(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
            Person = new PersonRepository(_dbContext);
            Service = new ServiceRepository(_dbContext);
            Experience = new ExperienceRepository(_dbContext);
            CustomerMessage = new CustomerMessageRepository(_dbContext);
            Education = new EducationRepository(_dbContext);
            Project = new ProjectRepository(_dbContext);
            ProjectAndTechnology = new ProjectAndTechnologyRepository(_dbContext);
            Technology = new TechnologyRepository(_dbContext);
            LocalUser = new UserRepository(_dbContext);
        }
        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}