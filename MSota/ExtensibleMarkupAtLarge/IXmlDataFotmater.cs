namespace MSota.ExtensibleMarkupAtLarge
{
    public interface IXmlDataFotmater
    {
        void BeginExtractKcbData(IXmlProps message);
        void BeginExtractMpesaData(IXmlProps message);
    }
}