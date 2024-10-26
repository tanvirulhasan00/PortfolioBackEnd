using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Portfolio.Models.IRepositoryRequestModels;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(IRepositoryRequest<T> request);
        Task<T> GetAsync(IRepositoryRequest<T> request);
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        void Remove(T entity);
        void RemoveRange(List<T> entities);
    }
}