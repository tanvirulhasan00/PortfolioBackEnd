using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.IServiceRepo
{
    public interface IServiceRepository : IRepository<Service>
    {
        bool IsUniqueUser();
        void Update(Service services);

    }
}