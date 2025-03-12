using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MGisbert.Appointments.Data.Repositories.Implementation
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly Context _context;

        public BaseRepository(Context context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T entity, bool saveChanges = true)
        {
            await _context.Set<T>().AddAsync(entity);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public virtual async Task AddAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity, bool saveChanges = true)
        {
            _context.Set<T>().Remove(entity);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<U>(Expression<Func<T, U>> include)
        {
            return await _context.Set<T>().Include(include).ToListAsync();
        }

        public virtual async Task<bool> GetAny(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).AnyAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> include)
        {
            return await _context.Set<T>().Include(include).Where(predicate).ToListAsync();
        }

        public virtual async Task<T> GetSingleASync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<T> GetSingleASync<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> include)
        {
            return await _context.Set<T>().Include(include).SingleOrDefaultAsync(predicate);
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity, bool saveChanges = true)
        {
            _context.Set<T>().Update(entity);
            if (saveChanges) await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(IEnumerable<T> entities, bool saveChanges = true)
        {
            _context.Set<T>().UpdateRange(entities);
            if (saveChanges) await _context.SaveChangesAsync();
        }
    }
}
