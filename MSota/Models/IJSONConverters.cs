namespace MSota.Models
{
    public interface IJSONConverters
    {
        Calendar_II AccountLedgerDeserializedJSON(string jsonString);
        string AccountLedgerSerializedJSON();
    }
}