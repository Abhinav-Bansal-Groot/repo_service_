using Microsoft.EntityFrameworkCore;
using repo_service.Data;
using System.Linq.Expressions;
using repo_service.UnitOfWork.Interface;

namespace repo_service.UnitOfWork.Services
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
