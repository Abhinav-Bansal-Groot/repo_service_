using Microsoft.EntityFrameworkCore.Storage;
using repo_service.Data;
using repo_service.Models.Entities;
//using repo_service.Repository.Interface;
//using repo_service.Repository.Services;
using repo_service.UnitOfWork.Interface;

namespace repo_service.UnitOfWork.Services
{
    public class UnitOfWork : IUnitOfWork
    { 
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        //public  IEmployeeRepository _Employees { get; }
        //public IDepartmentInterface Departments {  get; }

        //public IGenericRepository <Employee> Employees { get; }
        //public IGenericRepository<Department> Departments { get; }

        //public UnitOfWork(AppDbContext context)
        //{
        //    _context = context;
        //    Employees = new GenericRepository<Employee>(_context);
        //    Departments = new GenericRepository<Department>(_context);
        //}

        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
            {
                return (IGenericRepository<T>)repo;
            }
            var newRepo = new GenericRepository<T>(_context);
            _repositories[typeof(T)] = newRepo;
            return newRepo;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // ASP.NET Core will automatically call .Dispose() at the end of each HTTP request
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
 