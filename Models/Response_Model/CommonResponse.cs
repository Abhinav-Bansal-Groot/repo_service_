using repo_service.Helpers;

namespace repo_service.Models.Response_Model
{
    public class CommonResponse<T>
    {
        public ResponseCodes Status { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }

    }
}
