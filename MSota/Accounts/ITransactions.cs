using MSota.Models;
using MSota.Responses;

namespace MSota.Accounts
{
    public interface ITransactions
    {
        TransactionsResponseII GetAllTransactionsII(string dateRangeJson);
        TransactionsResponse GetAllTransactions(Calendar cal);

        TransactionsResponse GetTransaction(TransactionModel tm);
    }
}
