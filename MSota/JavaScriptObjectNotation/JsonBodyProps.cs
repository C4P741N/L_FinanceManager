namespace MSota.JavaScriptObjectNotation
{
    public class JsonBodyProps
    {
        public string? DocType { get; set; }
        public string? Body { get; set; }
        public string? sender { get; set; }
        public DateTime DocDateTime { get; set; }
        public long LongDate { get; set; }
        public int IsRead { get; set; }
        public int type { get; set; }
        public int thread { get; set; }
        public string? Service_center { get; set; }
        public JsonSmsProps smsProps { get; set; } = new JsonSmsProps();
    }
}
