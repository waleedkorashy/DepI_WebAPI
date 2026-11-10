using LibraryApp.Data;
using LibraryApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Repositories
{
    public sealed class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public Task AddAsync(T entity)
        {
            return _dbSet.AddAsync(entity).AsTask();
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object?>>[] includes)
        {
            return await ApplyIncludes(_dbSet.AsQueryable(), includes).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes)
        {
            return await ApplyIncludes(_dbSet.AsQueryable(), includes)
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>>? predicate = null,
            int skip = 0,
            int? take = null,
            params Expression<Func<T, object?>>[] includes)
        {
            IQueryable<T> query = ApplyIncludes(_dbSet.AsQueryable(), includes);
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate is null
                ? _dbSet.CountAsync()
                : _dbSet.CountAsync(predicate);
        }

        private static IQueryable<T> ApplyIncludes(
            IQueryable<T> query,
            IEnumerable<Expression<Func<T, object?>>> includes)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}
