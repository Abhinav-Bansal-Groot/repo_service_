namespace repo_service.Models.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        required
        public string Name { get; set; } = null!;

        required
        public string Email { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
