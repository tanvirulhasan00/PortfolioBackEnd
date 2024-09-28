using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, string? includeProperties = null, bool tracked = false);
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(List<T> entities);
    }
}