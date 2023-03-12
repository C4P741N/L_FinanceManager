using Microsoft.IdentityModel.Tokens;
using MSota.DataLibrary;

namespace MSota.Models
{
    public class TransactionsResponse : ITransactionsResponse
    {
        ITransactions trs;

        public TransactionsResponse(ITransactions trs, List<ITransactions> transactions)
        {
            this.trs = trs;
            Transactions = transactions;
        }

        //public TransactionsResponse(ITransactions transactions)
        //{
        //    trs = transactions;
        //}

        public List<ITransactions> Transactions { get; set; }

        public ITransactions CollectTransactions()
        {
            foreach (var dXml in DbAccessor.LoadTransactionStatistics)
            {
                var tr = new Transaction();

                if (dXml.FulizaCharge.IsNullOrEmpty() == false)
                    dXml.TransactionCost = dXml.FulizaCharge;

                tr.TransactionID = dXml.Code_ID;
                tr.TranactionQuota = dXml.Quota;
                tr.TranactionDate = long.Parse(dXml.Date);

                //tr.TransactionAmount = math.RoundingOf(Convert.ToDouble(dXml.CashAmount));
                //tr.TranactionCharge = math.RoundingOf(Convert.ToDouble(dXml.TransactionCost));
                //tr.LoanBorrowed = math.RoundingOf(Convert.ToDouble(dXml.FulizaBorrowed));

                trs.AddTransaction(tr);
            }

            return trs;
        }
    }
}
