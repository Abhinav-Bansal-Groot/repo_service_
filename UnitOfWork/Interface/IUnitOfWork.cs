using Microsoft.EntityFrameworkCore;
//using repo_service.Repository.Interface;
using repo_service.Models.Entities;


namespace repo_service.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenericRepository<Employee> Employees { get; }
        //IGenericRepository<Department> Departments { get; }
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task<int> SaveAsync();
    }
}
