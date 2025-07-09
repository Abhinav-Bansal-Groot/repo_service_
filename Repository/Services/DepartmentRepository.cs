//using repo_service.Data;
//using repo_service.Models.Entities;
//using repo_service.Repository.Interface;
//using Microsoft.EntityFrameworkCore;

//namespace repo_service.Repository.Services
//{
//    public class DepartmentRepository : IDepartmentInterface
//    {
//        private readonly AppDbContext _context;

//        public DepartmentRepository(AppDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<IEnumerable<Department>> GetDepartments()
//        {
//            return await _context.Departmnets.ToListAsync(); 
//        }
//    }
//}
