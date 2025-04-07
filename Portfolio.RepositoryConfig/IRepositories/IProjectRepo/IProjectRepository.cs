using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.IProjectRepo
{
    public interface IProjectRepository : IRepository<Project>
    {
        void Update(Project projects);
    }
}