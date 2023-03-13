namespace MSota.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Error Error { get; set; }

        public BaseResponse(bool success, Error error)
        {
                Success = !error.bErrorFound;
                Message = error.szErrorMessage;
                Error = error;
        }
    }
}
