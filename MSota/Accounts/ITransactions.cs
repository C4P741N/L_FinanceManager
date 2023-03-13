using MSota.Models;
using MSota.Responses;

namespace MSota.Accounts
{
    public interface ITransactions
    {

        MSota.Responses.TransactionsResponse GetAllTransactions();

        MSota.Responses.TransactionsResponse GetTransaction(TransactionModel tm);
    }
}
