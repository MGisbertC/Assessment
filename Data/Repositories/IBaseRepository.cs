using System.Linq.Expressions;

namespace MGisbert.Appointments.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<T> GetSingleASync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleASync<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> include);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAsync<U>(Expression<Func<T, bool>> predicate, Expression<Func<T, U>> include);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync<U>(Expression<Func<T, U>> include);
        Task<bool> GetAny(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity, bool saveChanges = true);
        Task AddAsync(IEnumerable<T> entities, bool saveChanges = true);
        Task UpdateAsync(T entity, bool saveChanges = true);
        Task UpdateAsync(IEnumerable<T> entities, bool saveChanges = true);
        Task DeleteAsync(T entity, bool saveChanges = true);
        Task<int> SaveChangesAsync();
        void Detach(T entity);
    }
}
