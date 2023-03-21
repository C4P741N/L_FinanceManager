using MSota.DataLibrary;
using MSota.Models;
using System;
using System.Diagnostics;

namespace MSota.Accounts
{
    public class Transactions : ITransactions
    {
        ISqlDataServer _sqlDataServer;
        public Transactions(ISqlDataServer sqlDataServer)
        {
            _sqlDataServer = sqlDataServer;
        }
        public Responses.TransactionsResponse GetAllTransactions()
        {
            List<TransactionModel> lsTransactions = new List<TransactionModel>();

            try
            {
                foreach (var dXml in _sqlDataServer.LoadTransactionStatistics())
                {
                    var tr = new TransactionModel();

                    //if (dXml.FulizaCharge.IsNullOrEmpty() == false)
                    //    dXml.TransactionCost = dXml.FulizaCharge;

                    //trs.TotalAmountTransacted += math.RoundingOf(Convert.ToDouble(dXml.CashAmount));
                    //trs.TotalLoanBorrowed += math.RoundingOf(Convert.ToDouble(dXml.FulizaBorrowed));
                    //trs.TotalCharge += math.RoundingOf(Convert.ToDouble(dXml.TransactionCost));

                    tr.TransactionID = dXml.Code_ID;
                    tr.TranactionQuota = dXml.Quota;
                    tr.TranactionDate = Convert.ToDateTime(dXml.Date);

                    tr.TransactionAmount = Convert.ToDouble(dXml.CashAmount);
                    tr.TranactionCharge = Convert.ToDouble(dXml.TransactionCost);
                    tr.LoanBorrowed = Convert.ToDouble(dXml.FulizaBorrowed);

                    lsTransactions.Add(tr);
                }

                return new Responses.TransactionsResponse(new MSota.Responses.Error(), lsTransactions);

            }
            catch (Exception ex)
            {

                return new MSota.Responses.TransactionsResponse(new MSota.Responses.Error
                {
                    szErrorMessage = ex.Message,
                    szStackTrace = ex.StackTrace,
                    bErrorFound = true
                }, lsTransactions);
            }

        }

        public Responses.TransactionsResponse GetTransaction(TransactionModel tm)
        {
            throw new NotImplementedException();
        }
    }
}
