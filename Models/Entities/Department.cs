using System.ComponentModel.DataAnnotations;

namespace repo_service.Models.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string? Dept_Name { get; set; }  
    }
}