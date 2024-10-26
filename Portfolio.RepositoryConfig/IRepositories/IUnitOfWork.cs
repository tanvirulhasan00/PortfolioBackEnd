using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IUnitOfWork
    {
        public IPersonRepository Person { get;}
        Task<int> Save();
    }
}