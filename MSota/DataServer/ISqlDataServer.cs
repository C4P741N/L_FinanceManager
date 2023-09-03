using MSota.ExtensibleMarkupAtLarge;
using MSota.Models;

namespace MSota.DataServer
{
    public interface ISqlDataServer
    {
        AccountLegerModel LoadAccountLegerSummary();
        void PostData(List<IXmlProps> x_vprop);
        void PostData(JsonBodyModel vals);
        List<TransactionModel> LoadTransactionStatistics(Calendar cal);
        List<FactionsModel> LoadFactionsStatistics(Calendar cal);
        List<FactionListModel> LoadFactionsList(string FactionID);
    }
}