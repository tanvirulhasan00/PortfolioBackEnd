using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.DbModels;

namespace Portfolio.RepositoryConfig.IRepositories.IPersonRepo
{
    public interface IPersonRepository : IRepository<Person>
    {
        bool IsUniqueUser();
        void Update(Person persons);

    }
}