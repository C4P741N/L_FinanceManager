using System.Net;

namespace MSota.Responses
{
    public class BaseResponse
    {
        public bool _success { get; set; }
        public string _message { get; set; }
        public Error _error { get; set; }
        //public HttpStatusCode statusCode { get; set; }
        //public HttpRequestMessage requestMessage { get; set; } //carries POST values?
        //HttpResponseMessage

        public BaseResponse(Error error)
        {
                _success = !error.bErrorFound;
                _message = error.szErrorMessage;
                _error = error;
        }
    }
}
