using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.RepositoryConfig.IRepositories.IPersonRepo;

namespace Portfolio.RepositoryConfig.Repositories.PersonRepo
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        private readonly PortfolioDbContext _context;
        public PersonRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public bool IsUniqueUser()
        {
            var person = _context.Persons.ToList();
            if (person.Count > 0)
            {
                return false;
            }
            return true;
        }

        public void Update(Person persons)
        {
            _context.Persons.Update(persons);
        }
    }
}