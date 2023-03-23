using MSota.DataLibrary;
using MSota.Models;
using System;
using System.Diagnostics;

namespace MSota.Accounts
{
    public class Transactions : ITransactions
    {
        ISqlDataServer _sqlDataServer;
        List<TransactionModel> lsTransactions = null;
        List<FactionsModel> lsFactions = null;
        public Transactions(ISqlDataServer sqlDataServer)
        {
            _sqlDataServer = sqlDataServer;
        }
        public Responses.TransactionsResponse GetAllTransactions()
        {
            lsTransactions = new List<TransactionModel>();
            lsFactions = new List<FactionsModel>();

            try
            {
                lsTransactions = _sqlDataServer.LoadTransactionStatistics();
                lsFactions = _sqlDataServer.LoadFactionsStatistics();

                return new Responses.TransactionsResponse(new MSota.Responses.Error(), lsTransactions, lsFactions);

            }
            catch (Exception ex)
            {

                return new MSota.Responses.TransactionsResponse(new MSota.Responses.Error
                {
                    szErrorMessage = ex.Message,
                    szStackTrace = ex.StackTrace,
                    bErrorFound = true
                }, lsTransactions, lsFactions);
            }

        }

        public Responses.TransactionsResponse GetTransaction(TransactionModel tm)
        {
            throw new NotImplementedException();
        }
    }
}
