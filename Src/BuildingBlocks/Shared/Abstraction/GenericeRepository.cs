using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace Shared.Abstraction
{
    public class GenericeRepository<T, DB> : IGenericeRepository<T, DB> where DB : DbContext where T : class
    {
        private readonly DB _context;
        public GenericeRepository(DB context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Set<T>().Add(entity);


        }


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _context.Set<T>().AddAsync(entity, cancellationToken);


        }

        public void Delete<TId>(TId id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
            _context.Set<T>().Remove(entity);

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().ToListAsync();
            return result;
        }
        public async Task<IQueryable<T>> GetAllAsQuerable()
        {
            var result = _context.Set<T>().AsNoTracking().AsQueryable();

            return result;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] Includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if ((query == null))
            {
                return null;
            }
            foreach (var include in Includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync<TId>(
           TId id,
           bool trackChanges = false,
           params Expression<Func<T, object>>[] includes)
           where TId : notnull
        {
            IQueryable<T> query = _context.Set<T>();
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            if (includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id));
        }

        public async Task<T> GetByIdAsync<TId>(TId id)
        {
            var entity = _context.Set<T>().FindAsync(id);
            return await entity;

        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Set<T>().Update(entity);


        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<IQueryable<T>> Query()
        {
            return _context.Set<T>();

        }

        public void DeleteRange<TId>(IEnumerable<TId> ids)
        {
            foreach (var id in ids)
            {
                var entity = _context.Set<T>().Find(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                }
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id), cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {

            var id = _context.Model.FindEntityType(typeof(T))
                .FindPrimaryKey().Properties.Select(x => x.PropertyInfo.GetValue(entity)).FirstOrDefault();

            if (id == null) throw new ArgumentException("Entity identity could not be determined.");


            var existingEntity = await _context.Set<T>().FindAsync(new[] { id }, cancellationToken);

            if (existingEntity == null)
            {
                throw new ArgumentException("Entity not found in database.");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
