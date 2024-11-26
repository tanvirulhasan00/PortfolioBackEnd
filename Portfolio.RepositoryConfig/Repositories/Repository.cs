using System.Net;
using Microsoft.EntityFrameworkCore;
using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.RequestModels.GenericRequestModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.RepositoryConfig.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PortfolioDbContext _context;
        internal readonly DbSet<T> _dbSet;
        public Repository(PortfolioDbContext context)
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAll(GenericRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? _dbSet.AsNoTracking() : _dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var property in request.IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                };
            }
            return await query.ToListAsync(request.CancellationToken);
        }
        public async Task<T> Get(GenericRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? _dbSet.AsNoTracking() : _dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var property in request.IncludeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                };
            }
            var result = await query.FirstOrDefaultAsync(request.CancellationToken) ?? throw new Exception($"{HttpStatusCode.InternalServerError}");
            return result;
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}