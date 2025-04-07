
using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.IEducationRepo
{
    public interface IEducationRepository : IRepository<Education>
    {
        void Update(Education educations);
    }
}