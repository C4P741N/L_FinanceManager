using MSota.ExtensibleMarkupAtLarge;
using MSota.Models;

namespace MSota.DataLibrary
{
    public interface ISqlDataServer
    {
        void PostData(List<IXmlProps> x_vprop);
        public List<TransactionModel> LoadTransactionStatistics();
    }
}