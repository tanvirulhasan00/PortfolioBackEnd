using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.ICustomerMessageRepo
{
    public interface ICustomerMessageRepository : IRepository<CustomerMessage>
    {
        void Update(CustomerMessage customerMessages);
    }
}