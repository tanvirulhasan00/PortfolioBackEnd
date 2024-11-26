using Portfolio.Models.RequestModels.GenericRequestModels;

namespace Portfolio.RepositoryConfig.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(GenericRequest<T> request);
        Task<T> Get(GenericRequest<T> request);
        Task AddAsync(T entity);
        void Remove(T entity);
    }
}