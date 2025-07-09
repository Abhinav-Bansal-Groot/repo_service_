//using Azure.Core;
//using Microsoft.EntityFrameworkCore;
//using repo_service.Data;
//using repo_service.Models.Entities;
//using repo_service.Models.Request_Model;
//using repo_service.Models.Response_Model;
//using repo_service.Repository.Interface;
//using System.Linq;

//namespace repo_service.Repository.Services
//{
//    public class EmployeeRepository : IEmployeeRepository
//    {
//        private readonly AppDbContext appDbContext;

//        public EmployeeRepository(AppDbContext appDbContext)
//        {
//            this.appDbContext = appDbContext;
//        }

//        public async Task<IEnumerable<Employee>> GetEmployees()
//        {
//            return await appDbContext.Employees.ToListAsync();
//        }

//        public async Task<Response_Model> GetEmployee(int employeeId)
//        {
//            //return await appDbContext.Employees
//            //    .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
//            var employee = new Employee();
//            employee = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

//            var response = new Response_Model
//            {
//                Id = employee.EmployeeId,
//                Name = employee.Name,
//                Email = employee.Email
//            };
//            return response;
//        }

//        public async Task<Response_Model> AddEmployee(Request_Model requests)
//        {
//            var employee = new Employee
//            {
//                Name = requests.Name,
//                Email = requests.Email
//            };

//            await appDbContext.Employees.AddAsync(employee);
//            await appDbContext.SaveChangesAsync();

//            var response = new Response_Model
//            {
//                Id = employee.EmployeeId,
//                Name = employee.Name,
//                Email = employee.Email
//            };
//            return response;
//        }

//        public async Task<IEnumerable<Response_Model>> SearchEmployees(string? name, string? email)
//        {
//            IQueryable<Employee> query = appDbContext.Employees.AsQueryable();

//            // Define a single predicate that we'll combine
//            Func<Employee, bool> predicate = e => true; // Start with always-true

//            if (!string.IsNullOrEmpty(name))
//            {
//                // Combine with existing
//                Func<Employee, bool> prev = predicate;
//                predicate = e => prev(e) && e.Name.Contains(name);
//            }

//            if (!string.IsNullOrEmpty(email))
//            {
//                Func<Employee, bool> prev = predicate;
//                predicate = e => prev(e) && e.Email.Contains(email);
//            }

//            var employees = await query.ToListAsync();
//            var filtered = employees.Where(predicate);

//            Func<Employee, Response_Model> mapToResponse = e => new Response_Model
//            {
//                Id = e.EmployeeId,
//                Name = e.Name,
//                Email = e.Email
//            };
//            return filtered.Select(mapToResponse);
//        }
//    }
//}