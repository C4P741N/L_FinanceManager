namespace MSota.JavaScriptObjectNotation
{
    public interface IJsonProps
    {
        List<SMSMessages> Properties { get; set; }
        public Dictionary<string, List<SMSMessages>>? value { get; set; }
    }
}