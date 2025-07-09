using Microsoft.EntityFrameworkCore;
using repo_service.Models.Entities;

namespace repo_service.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }  
}
