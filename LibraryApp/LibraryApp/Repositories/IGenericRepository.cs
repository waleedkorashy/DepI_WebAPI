using LibraryApp.Entities;
using System.Linq.Expressions;

namespace LibraryApp.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes);
        Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
            int skip = 0,
            int? take = null,
            params Expression<Func<T, object?>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
