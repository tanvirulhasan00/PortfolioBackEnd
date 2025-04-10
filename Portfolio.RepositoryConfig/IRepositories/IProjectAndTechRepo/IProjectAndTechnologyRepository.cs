using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.IProjectAndTechRepo
{
    public interface IProjectAndTechnologyRepository : IRepository<ProjectAndTechnology>
    {
        void Update(ProjectAndTechnology projectAndTechnologies);
    }
}