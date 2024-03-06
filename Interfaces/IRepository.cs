using SRMS.Models.Temp;

namespace SRMS.Interfaces
{
    public interface IRepository<T>
    {
        Task Update(T entity);
        Task Delete(T entity);
        Task Add(T entity);

        Task<string?> GetId(string? type, AuthenticationOptions? options);

        Task<T> Get(string? type, AuthenticationOptions? options);

        Task<IEnumerable<T>> GetAll(PagingOptions options);

        Task<IEnumerable<T>> GetAll(string? type, string? id,PagingOptions options);

        Task<IEnumerable<T>> FindAll(QueryFilterOptions? filter);

        Task<IEnumerable<T>> FindAll(string? type, string? id,QueryFilterOptions? filter);

        Task<bool> Has(T entity);

        Task<bool> Authenticate(string? type, AuthenticationOptions? options);

        Task<bool> CanUpdate(T entity);

        Task<int> Count();

        Task<int?> Count(string? type, string? id);

        Task<int> Count(string? query);

        Task<int?> Count(string? type, string? id,string? query);

        Task Delete(string? type, string? id);

        Task<T> Get(string? type, string? id);

    }
}
