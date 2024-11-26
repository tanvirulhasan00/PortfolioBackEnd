using Portfolio.RepositoryConfig.IRepositories.ICustomerMessageRepo;
using Portfolio.RepositoryConfig.IRepositories.IEducationRepo;
using Portfolio.RepositoryConfig.IRepositories.IExperienceRepo;
using Portfolio.RepositoryConfig.IRepositories.IPersonRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectAndTechRepo;
using Portfolio.RepositoryConfig.IRepositories.IProjectRepo;
using Portfolio.RepositoryConfig.IRepositories.IServiceRepo;
using Portfolio.RepositoryConfig.IRepositories.ITechnologyRepo;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IUnitOfWork
    {
        public IPersonRepository Person { get; }
        public IServiceRepository Service { get; }
        public IExperienceRepository Experience { get; }
        public ICustomerMessageRepository CustomerMessage { get; }
        public IEducationRepository Education { get; }
        public IProjectRepository Project { get; }
        public IProjectAndTechnologyRepository ProjectAndTechnology { get; }
        public ITechnologyRepository Technology { get; }

        Task<int> Save();
    }
}