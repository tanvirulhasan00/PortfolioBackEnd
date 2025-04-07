using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.PortfolioModels;
using Portfolio.RepositoryConfig.IRepositories.IServiceRepo;

namespace Portfolio.RepositoryConfig.Repositories.ServiceRepo
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly PortfolioDbContext _context;
        public ServiceRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsUniqueUser()
        {
            var service = _context.Services.ToList();
            if (service.Count > 0)
            {
                return false;
            }
            return true;
        }

        public void Update(Service services)
        {
            _context.Services.Update(services);
        }
    }
}