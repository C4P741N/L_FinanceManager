using MSota.Models;

namespace MSota.Extractors
{
    public interface IEx_SMS
    {
        JsonSmsModel MessageExtractBegin(JsonBodyModel vals);
    }
}