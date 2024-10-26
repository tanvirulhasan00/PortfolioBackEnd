using System.Security.Cryptography.X509Certificates;
using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.ApiRequestModels.Person;
using Portfolio.Models.PortfolioModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.RepositoryConfig.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly PortfolioDbContext _dbContext;
        public PersonRepository(PortfolioDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsUserUnique(){
            var id = _dbContext.Persons.Select(s=>s.PersonId);
            if(id.Any()){
                return false;
            }
            return true;
        }

        public void Update(Person persons)
        {
            _dbContext.Update(persons);
        }

        public void UpdateRange(List<Person> persons)
        {
            _dbContext.UpdateRange(persons);
        }
    }
}