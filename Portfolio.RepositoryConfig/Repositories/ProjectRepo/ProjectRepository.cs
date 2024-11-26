using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.RepositoryConfig.IRepositories.IProjectRepo;



namespace Portfolio.RepositoryConfig.Repositories.ProjectRepo
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly PortfolioDbContext _context;
        public ProjectRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Project projects)
        {
            _context.Projects.Update(projects);
        }
    }
}