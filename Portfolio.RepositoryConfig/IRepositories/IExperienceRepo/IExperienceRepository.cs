using Portfolio.Models.DbModels;

namespace Portfolio.RepositoryConfig.IRepositories.IExperienceRepo
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        void Update(Experience experiences);
    }
}