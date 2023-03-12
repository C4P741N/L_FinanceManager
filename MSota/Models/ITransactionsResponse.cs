using MSota.DataLibrary;

namespace MSota.Models
{
    public interface ITransactionsResponse
    {
        List<ITransactions> Transactions { get; set; }

        ITransactions CollectTransactions();
    }
}