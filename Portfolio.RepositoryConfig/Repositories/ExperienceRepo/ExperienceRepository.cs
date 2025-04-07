using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.PortfolioModels;
using Portfolio.RepositoryConfig.IRepositories.IExperienceRepo;

namespace Portfolio.RepositoryConfig.Repositories.ExperienceRepo
{
    public class ExperienceRepository : Repository<Experience>, IExperienceRepository
    {
        private readonly PortfolioDbContext _context;
        public ExperienceRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Experience experiences)
        {
            _context.Experiences.Update(experiences);
        }
    }
}