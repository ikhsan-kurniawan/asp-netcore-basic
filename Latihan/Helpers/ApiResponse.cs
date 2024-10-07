namespace Latihan.Helpers
{
    public class ApiResponse
    {
        //public int StatusCode { get; set; }
        //public string Message { get; set; }
        //public object? Data { get; set; }

        public static object CreateResponse(int statusCode, string message, object data)
        {
            return new
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static object CreateResponse(int statusCode, string message)
        {
            return new 
            {
                StatusCode = statusCode,
                Message = message,
            };
        }
    }
}
