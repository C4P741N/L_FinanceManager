namespace MSota.DataLibrary
{
    public interface ITransactions
    {
        Transaction AddTransaction(Transaction tr);
        IEnumerator<Transaction> GetEnumerator();
        Transaction GetTransaction(string szTransactionID);
        Transaction RemoveTransaction(Transaction tr);
        bool TransactionExist(string szTransactionID);
    }
}