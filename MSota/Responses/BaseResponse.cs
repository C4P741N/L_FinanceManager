using System.Net;

namespace MSota.Responses
{
    public class BaseResponse
    {
        public Error _error { get; set; }
        public HttpStatusCode _statusCode { get; set; }

        public BaseResponse(Error error, HttpStatusCode statusCode)
        {
            _error = error;
            _statusCode = statusCode;
        }
    }
}
