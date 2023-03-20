namespace MSota.Responses
{
    public class Error
    {
        public string szStackTrace { get; set; } = string.Empty;
        public string szErrorMessage { get; set; } = string.Empty;
        public bool bErrorFound { get; set; }
    }
}
