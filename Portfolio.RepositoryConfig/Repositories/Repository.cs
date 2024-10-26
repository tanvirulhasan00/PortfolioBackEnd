using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.IRepositoryRequestModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.RepositoryConfig.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PortfolioDbContext _dbContext;
        internal readonly DbSet<T> dbSet;
        public Repository(PortfolioDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task<List<T>> GetAllAsync(IRepositoryRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? dbSet.AsNoTracking() : dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var includeProperty in request.IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync(request.CancellationToken);
        }

        public async Task<T> GetAsync(IRepositoryRequest<T> request)
        {
            IQueryable<T> query = request.NoTracking == true ? dbSet.AsNoTracking() : dbSet;
            if (request.Expression != null)
            {
                query = query.Where(request.Expression);
            }
            if (request.IncludeProperties != null)
            {
                foreach (var includeProperty in request.IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            var data = await query.FirstOrDefaultAsync(request.CancellationToken) ?? throw new InvalidOperationException("Entity not found.");
            return data;
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}