using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.DatabaseConfig.Data;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.RepositoryConfig.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PortfolioDbContext _dbContext;
        public IPersonRepository Person { get; private set; }

        public UnitOfWork(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
            Person = new PersonRepository(_dbContext);
        }
        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}