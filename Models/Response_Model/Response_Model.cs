namespace repo_service.Models.Response_Model
{
    public class Response_Model
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int DepartmentId { get; set; }
    }
}
