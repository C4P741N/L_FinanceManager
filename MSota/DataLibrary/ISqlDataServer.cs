using MSota.ExtensibleMarkupAtLarge;

namespace MSota.DataLibrary
{
    public interface ISqlDataServer
    {
        void PostData(List<IXmlProps> x_vprop);
    }
}