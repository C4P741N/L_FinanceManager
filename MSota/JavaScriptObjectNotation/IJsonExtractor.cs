using MSota.Responses;

namespace MSota.JavaScriptObjectNotation
{
    public interface IJsonExtractor
    {
        BaseResponse ExtractBegin(string sms);
    }
}