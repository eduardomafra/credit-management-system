namespace CustomerService.Application.DTOs
{
    public class ApiResponse<T>
    {        
        public ApiResponse(T result)
        {
            Success = true;
            StatusCode = 200;
            Result = result;
            Errors = null;
        }

        public ApiResponse(IEnumerable<string> errors = null)
        {
            Success = false;
            StatusCode = 500;
            Errors = errors;
        }

        public ApiResponse(bool success, int statusCode, T result, IEnumerable<string> errors = null)
        {
            Success = success;
            StatusCode = statusCode;
            Result = result;
            Errors = errors;
        }

        public bool Success { get; set; }
        public T Result { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
