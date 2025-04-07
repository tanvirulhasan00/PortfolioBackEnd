using Portfolio.Models.ApiRequestModels.GenericRequestModels;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(GenericRequest<T> request);
        Task<T> GetAsync(GenericRequest<T> request);
        Task AddAsync(T entity);
        void Remove(T entity);

    }
}