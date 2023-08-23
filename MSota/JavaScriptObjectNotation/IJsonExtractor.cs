using MSota.Responses;

namespace MSota.JavaScriptObjectNotation
{
    public interface IJsonExtractor
    {
        BaseResponse UpdateFromJson(string sms);
    }
}