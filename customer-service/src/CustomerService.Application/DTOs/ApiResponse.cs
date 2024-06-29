namespace CustomerService.Application.DTOs
{
    public class ApiResponse<T>
    {        
        public ApiResponse(T result, int statusCode = 200)
        {
            Success = true;
            StatusCode = statusCode;
            Result = result;
            Errors = null;
        }

        public ApiResponse(IEnumerable<string> errors = null, int statusCode = 500)
        {
            Success = false;
            StatusCode = 500;
            Errors = errors;
            Result = default;
        }

        public ApiResponse(bool success, int statusCode, T result, IEnumerable<string> errors = null)
        {
            Success = success;
            StatusCode = statusCode;
            Result = result;
            Errors = errors;
        }

        public bool Success { get; set; }
        public T? Result { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
