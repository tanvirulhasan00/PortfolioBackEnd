using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.IExperienceRepo
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        void Update(Experience experiences);
    }
}