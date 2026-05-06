using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Shared.Abstraction
{
    public interface IGenericeRepository<T, DB> where T : class where DB : DbContext
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes);
        Task<T> GetByIdAsync<TId>(TId id,
           bool trackChanges = false,
           params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity,CancellationToken cancellationToken=default);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task<IQueryable<T>> GetAllAsQuerable();
        Task<IQueryable<T>> Query();
        Task<T> GetByIdAsync<TId>(TId id);
        Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken);
        void Add(T entity);
        void Update(T entity);
        void Delete<TId>(TId id);
        void DeleteRange<TId>(IEnumerable<TId> ids);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();

    }
}
