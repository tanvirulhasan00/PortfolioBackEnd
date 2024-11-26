using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.RepositoryConfig.IRepositories.IEducationRepo;


namespace Portfolio.RepositoryConfig.Repositories.EducationRepo
{
    public class EducationRepository : Repository<Education>, IEducationRepository
    {
        private readonly PortfolioDbContext _context;
        public EducationRepository(PortfolioDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Education educations)
        {
            _context.Educations.Update(educations);
        }
    }
}