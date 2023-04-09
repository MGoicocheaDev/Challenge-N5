using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using web_api_lib_data.Context;

namespace web_api_lib_application.Infraestructure.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly WebApiContext _context;
        private DbSet<T> dbSet;

        public GenericRepository(WebApiContext context)
        {
            _context = context;
        }

        protected DbSet<T> Set
        {
            get { return dbSet ?? (dbSet = _context.Set<T>()); }
        }

        public void Add(T entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(T entity)
        {
            await Set.AddAsync(entity);
        }

        public T FindById(object id)
        {
            return FindByIdAsync(id).Result;
        }

        public async Task<T> FindByIdAsync(object id)
        {
            return await Set.FindAsync(id);
        }

        public async Task<T> FindByIdAsync(CancellationToken cancellationToken, object id)
        {
            return await Set.FindAsync(id, cancellationToken);
        }

        public IEnumerable<T> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Set.ToListAsync(cancellationToken);
        }

        public IEnumerable<T> GetAllByExpression(Expression<Func<T, bool>> predicate)
        {
            return GetAllByExpressionAsync(predicate).Result;
        }

        public async Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate)
        {
            return await Set.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByExpressionAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate)
        {
            return await Set.Where(predicate).ToListAsync(cancellationToken);
        }

        public IEnumerable<T> PageAll(int skip, int take)
        {
            return PageAllAsync(skip, take).Result;
        }

        public async Task<IEnumerable<T>> PageAllAsync(int skip, int take)
        {
            return await Set.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take)
        {
            return await Set.Skip(skip).Take(take).ToListAsync(cancellationToken);
        }

        public void Remove(T entity)
        {
            Set.Remove(entity);
        }

        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Set.Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
        }
    }
}
