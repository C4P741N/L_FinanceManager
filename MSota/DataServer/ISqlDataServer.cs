using MSota.ExtensibleMarkupAtLarge;
using MSota.JavaScriptObjectNotation;
using MSota.Models;

namespace MSota.DataServer
{
    public interface ISqlDataServer
    {
        void PostData(List<IXmlProps> x_vprop);
        void PostData(JsonBodyProps vals);
        List<TransactionModel> LoadTransactionStatistics(Calendar cal);
        List<FactionsModel> LoadFactionsStatistics(Calendar cal);
        List<FactionListModel> LoadFactionsList(string FactionID);
    }
}