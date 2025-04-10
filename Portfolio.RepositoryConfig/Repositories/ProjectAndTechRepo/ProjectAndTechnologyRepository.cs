using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.PortfolioModels;
using Portfolio.RepositoryConfig.IRepositories.IProjectAndTechRepo;



namespace Portfolio.RepositoryConfig.Repositories.ProjectAndTechRepo
{
    public class ProjectAndTechnologyRepository : Repository<ProjectAndTechnology>, IProjectAndTechnologyRepository
    {
        private readonly PortfolioDbContext _context;
        public ProjectAndTechnologyRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ProjectAndTechnology projectAndTechnologies)
        {
            _context.Update(projectAndTechnologies);
        }
    }
}