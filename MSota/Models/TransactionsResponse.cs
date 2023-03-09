namespace MSota.Models
{
    public class TransactionsResponse
    {
        public double TotalAmount { get; set; }
        public int TransactionCollections { get; set; }
        public TransactionsResponse()
        {
            Transactions = new List<L_Transaction>();
        }

        public List<L_Transaction> Transactions { get; set; }
    }
}
