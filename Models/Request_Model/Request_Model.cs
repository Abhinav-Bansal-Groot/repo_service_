namespace repo_service.Models.Request_Model
{
    public class Request_Model
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public int DepartmentId { get; set; }
    }
}
