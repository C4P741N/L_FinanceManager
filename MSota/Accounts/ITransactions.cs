using MSota.Models;
using MSota.Responses;

namespace MSota.Accounts
{
    public interface ITransactions
    {
        TransactionsResponseII GetAllTransactionsII();
        TransactionsResponse GetAllTransactions(Calendar cal);

        TransactionsResponse GetTransaction(TransactionModel tm);
    }
}
