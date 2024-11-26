using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
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

        // public void Update(ProjectAndTechnology projectAndTechnologies)
        // {
        //     _context.ProjectAndTechnologies.Update(projectAndTechnologies);
        // }
    }
}