using Microsoft.AspNetCore.Mvc;
using repo_service.Helpers;
using repo_service.Models.Entities;
using repo_service.Models.Response_Model;
using repo_service.UnitOfWork.Interface;

namespace repo_service.Controllers
{
    [Route("api/")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            try
            {
                var departmentRepo = _unitOfWork.GetRepository<Department>();
                var departments = await departmentRepo.GetAllAsync();

                var totalRecords = departments.Count();
                var results = departments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


                return Ok(new CommonResponse<IEnumerable<Department>>
                {
                    Status = ResponseCodes.Success,
                    Message = Messages.DepartmentsRetrieved,
                    Data = results
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
    }
}
