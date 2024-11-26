using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.RepositoryConfig.IRepositories.IServiceRepo;
using Portfolio.RepositoryConfig.IRepositories.ITechnologyRepo;

namespace Portfolio.RepositoryConfig.Repositories.ServiceRepo
{
    public class TechnologyRepository : Repository<Technology>, ITechnologyRepository
    {
        private readonly PortfolioDbContext _context;
        public TechnologyRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Technology technologies)
        {
            _context.Technologies.Update(technologies);
        }
    }
}