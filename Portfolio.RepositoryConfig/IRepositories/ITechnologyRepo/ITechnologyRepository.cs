using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.PortfolioModels;

namespace Portfolio.RepositoryConfig.IRepositories.ITechnologyRepo
{
    public interface ITechnologyRepository : IRepository<Technology>
    {
        void Update(Technology technologies);
    }
}