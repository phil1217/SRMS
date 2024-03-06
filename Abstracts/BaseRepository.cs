using Microsoft.EntityFrameworkCore;
using SRMS.Interfaces;
using SRMS.Models.Temp;

namespace SRMS.Abstracts
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> dbSet;
        protected DbContext dbContext;

        public BaseRepository(DbContext dbContext,DbSet<T> dbSet)
        {
            this.dbSet = dbSet;
            this.dbContext = dbContext;
        }

        public virtual async Task<int> Count()
        {

            return await dbSet.CountAsync();
        }

        public virtual async Task Add(T entity)
        {
            dbSet.Add(entity);
            await dbContext?.SaveChangesAsync();
        }

        public virtual async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await dbContext?.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            dbSet.Update(entity);
            await dbContext?.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAll(PagingOptions? page)
        {
            return await dbSet.Skip((page.Index ?? 0) * (page.Size ?? 0))
                                       .Take(page.Size ?? 0)
                                       .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAll(QueryFilterOptions? filter)
        {
            return null;
        }

        public virtual async Task<bool> Has(T entity)
        {
            return false;
        }

        public virtual async Task<bool> Authenticate(string? type, AuthenticationOptions? options)
        {
            return false;
        }

        public virtual async Task<bool> CanUpdate(T entity)
        {
            return false;
        }


        public virtual async Task Delete(string? type, string? id)
        {
        }

        public virtual async Task<int> Count(string? query)
        {
            return 0;
        }

        public virtual async Task<IEnumerable<T>> GetAll(string? type, string? id, PagingOptions options)
        {
            return null;
        }

        public virtual async Task<IEnumerable<T>> FindAll(string type, string? id, QueryFilterOptions? filter)
        {
            return null;
        }

        public virtual async Task<string?> GetId(string? type, AuthenticationOptions? options)
        {
            return null;
        }

        public virtual async Task<int?> Count(string? type, string? id)
        {
            return null;
        }

        public virtual async Task<int?> Count(string? type, string? id, string? query)
        {
            return null;
        }

        public virtual async Task<T> Get(string? type, AuthenticationOptions? options)
        {
            return null;
        }
        
        public virtual async Task<T> Get(string? type, string? id)
        {
            return null;
        }
    
    }
}
