using Portfolio.Models.ApiRequestModels.Person;
using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        bool IsUserUnique();
        void Update(Person persons);
        void UpdateRange(List<Person> persons);
    }
}