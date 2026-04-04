using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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




        public void Delete(int id)
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

        public async Task<T> GetByIdAsync(int id, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable().AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            return entity;

        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = _context.Set<T>().FindAsync(id);
            return await entity;
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.Entry(entity).State = EntityState.Modified;


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
    }
}
