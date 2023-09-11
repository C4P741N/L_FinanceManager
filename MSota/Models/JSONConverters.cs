using Newtonsoft.Json;

namespace MSota.Models
{
    public class JSONConverters : IJSONConverters
    {
        public Calendar_II AccountLedgerDeserializedJSON(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return null;

            return JsonConvert.DeserializeObject<Calendar_II>(jsonString);
        }

        public string AccountLedgerSerializedJSON()
        {
            return JsonConvert.SerializeObject(new object());
        }
    }
}
