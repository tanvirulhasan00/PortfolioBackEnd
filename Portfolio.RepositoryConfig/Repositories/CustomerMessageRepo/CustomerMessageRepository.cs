using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.RepositoryConfig.IRepositories.ICustomerMessageRepo;



namespace Portfolio.RepositoryConfig.Repositories.CustomerMessageRepo
{
    public class CustomerMessageRepository : Repository<CustomerMessage>, ICustomerMessageRepository
    {
        private readonly PortfolioDbContext _context;
        public CustomerMessageRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(CustomerMessage customerMessages)
        {
            _context.CustomerMessages.Update(customerMessages);
        }
    }
}