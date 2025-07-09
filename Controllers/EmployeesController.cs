using Microsoft.AspNetCore.Mvc;
using repo_service.Helpers;
using repo_service.Models.Entities;
using repo_service.Models.Request_Model;
using repo_service.Models.Response_Model;
using repo_service.UnitOfWork.Interface;
using System.Linq.Expressions;

namespace repo_service.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Employees")]
        public async Task<ActionResult<CommonResponse<IEnumerable<Response_Model>>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var employeeRepo = _unitOfWork.GetRepository<Employee>();
                var employees = await employeeRepo.GetAllAsync();

                var totalRecords = employees.Count();
                var results = employees
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new Response_Model
                    {
                        Id = e.EmployeeId,
                        Name = e.Name,
                        Email = e.Email,
                        DepartmentId = e.DepartmentId
                    })
                    .ToList();
                var paginatedResult = new PaginatedResponse<Response_Model>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    Data = results
                };
                //var results = employees.Select(e => new Response_Model
                //{
                //    Id = e.EmployeeId,
                //    Name = e.Name,
                //    Email = e.Email,
                //    DepartmentId = e.DepartmentId
                //});

                return Ok(new CommonResponse<PaginatedResponse<Response_Model>>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeesRetrieved,
                    Data = paginatedResult
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError,
                        Message = Messages.ErrorRetrievingData,
                        Data = null
                    });
            }
        }

        [HttpGet("Employees/{id:int}")]
        public async Task<ActionResult<CommonResponse<Response_Model>>> GetEmployee(int id)
        {
            try
            {
                var employeeRepo = _unitOfWork.GetRepository<Employee>();
                var employee = await employeeRepo.GetByIdAsync(id);

                if (employee == null)
                    return NotFound(new CommonResponse<string>
                    {
                        Status = ResponseCodes.NotFound,
                        Message = Messages.ErrorRetrievingData,
                        Data = null
                    });

                var response = new Response_Model
                {
                    Id = employee.EmployeeId,
                    Name = employee.Name,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId
                };

                return Ok(new CommonResponse<Response_Model>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeeRetrieved,
                    Data = response
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError,
                        Message = Messages.ErrorRetrievingData,
                        Data = null
                    });
            }
        }

        [HttpPost("Employee")]
        public async Task<ActionResult<CommonResponse<Response_Model>>> CreateEmployee([FromBody] Request_Model request)
        {
            if (request == null)
                return BadRequest(new CommonResponse<string>
                {
                    Status = ResponseCodes.ErrorCode,
                    Message = Messages.InvalidRequest,
                    Data = null
                });

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email))
                    throw new ArgumentNullException(Messages.InvalidRequest);

                var departmentRepo = _unitOfWork.GetRepository<Department>();
                var department = await departmentRepo.GetByIdAsync(request.DepartmentId);
                if (department == null)
                    throw new Exception(Messages.InvalidDepartmentId);

                var employeeRepo = _unitOfWork.GetRepository<Employee>();
                var employee = new Employee
                {
                    Name = request.Name,
                    Email = request.Email,
                    DepartmentId = department.DepartmentId
                };

                await employeeRepo.AddAsync(employee);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();

                var response = new Response_Model
                {
                    Id = employee.EmployeeId,
                    Name = employee.Name,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId
                };

                var result = new CommonResponse<Response_Model>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeeCreated,
                    Data = response
                };

                return result;
                    
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError,
                        Message = $"{Messages.ErrorCreatingEmployee} {ex.Message}",
                        Data = null
                    });
            }
        }

        [HttpPut("Employees/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Request_Model request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var employeeRepo = _unitOfWork.GetRepository<Employee>();
                var existing = await employeeRepo.GetByIdAsync(id);

                if (existing == null)
                    return NotFound(new CommonResponse<string>
                    {
                        Status = ResponseCodes.NotFound,
                        Message = Messages.ErrorRetrievingData,
                        Data = null
                    });

                existing.Name = request.Name;
                existing.Email = request.Email;

                await employeeRepo.Update(existing);
                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();

                return Ok(new CommonResponse<string>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeeUpdated,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError        ,
                        Message = $"{Messages.ErrorUpdatingEmployee} {ex.Message}",
                        Data = null
                    });
            }
        }

        [HttpPost("Employees/TransactionalBatchAdd")]
        public async Task<IActionResult> TransactionalBatchAdd([FromBody] List<Request_Model> requests)
        {
            if (requests == null || !requests.Any())
                return BadRequest(new CommonResponse<string>
                {
                    Status = ResponseCodes.ErrorCode,
                    Message = Messages.InvalidRequest,
                    Data = null
                });

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var employeeRepo = _unitOfWork.GetRepository<Employee>();
                var departmentRepo = _unitOfWork.GetRepository<Department>();

                foreach (var req in requests)
                {
                    if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Email))
                        throw new ArgumentNullException(Messages.InvalidRequest);

                    var dept = await departmentRepo.GetByIdAsync(req.DepartmentId);
                    if (dept == null)
                        throw new Exception(Messages.InvalidDepartmentId);

                    var emp = new Employee
                    {
                        Name = req.Name,
                        Email = req.Email,
                        DepartmentId = dept.DepartmentId
                    };

                    await employeeRepo.AddAsync(emp);
                }

                await _unitOfWork.SaveAsync();
                await _unitOfWork.CommitAsync();

                return Ok(new CommonResponse<string>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeesBatchAdded,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError,
                        Message = $"{Messages.ErrorBatchInsert} {ex.Message}",
                        Data = null
                    });
            }
        }

        [HttpGet("Employees/search")]
        public async Task<ActionResult<CommonResponse<IEnumerable<Response_Model>>>> SearchEmployees(
            [FromQuery] string? name,
            [FromQuery] string? email)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email))
                return BadRequest(new CommonResponse<string>
                {
                    Status = ResponseCodes.ErrorCode,
                    Message = Messages.MissingSearchParameter,
                    Data = null
                });

            try
            {
                var employeeRepo = _unitOfWork.GetRepository<Employee>();

                Expression<Func<Employee, bool>> predicate = e => true;

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
                    predicate = e => e.Name.Contains(name) && e.Email.Contains(email);
                else if (!string.IsNullOrWhiteSpace(name))
                    predicate = e => e.Name.Contains(name);
                else if (!string.IsNullOrWhiteSpace(email))
                    predicate = e => e.Email.Contains(email);

                var employees = await employeeRepo.FindAsync(predicate);

                var results = employees.Select(e => new Response_Model
                {
                    Id = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    DepartmentId = e.DepartmentId
                });

                return Ok(new CommonResponse<IEnumerable<Response_Model>>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.EmployeeSearchResults,
                    Data = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new CommonResponse<string>
                    {
                        Status = ResponseCodes.InternalError,
                        Message = $"{Messages.ErrorRetrievingData} {ex.Message}",
                        Data = null
                    });
            }
        }
    }
}
