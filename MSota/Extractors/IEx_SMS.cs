using MSota.JavaScriptObjectNotation;

namespace MSota.Extractors
{
    public interface IEx_SMS
    {
        SmsProps MessageExtractBegin(string szKey, Values sms);
    }
}