using Microsoft.AspNetCore.Http;
using MSota.DataServer;
using MSota.Models;
using System;
using System.Diagnostics;
using System.Net;

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
        public Responses.TransactionsResponse GetAllTransactions(Calendar cal)
        {
            lsTransactions = new List<TransactionModel>();
            lsFactions = new List<FactionsModel>();

            try
            {
                lsTransactions = _sqlDataServer.LoadTransactionStatistics(cal);
                lsFactions = _sqlDataServer.LoadFactionsStatistics(cal);

                return new Responses.TransactionsResponse(new MSota.Responses.Error(), lsTransactions, lsFactions, System.Net.HttpStatusCode.Accepted);

            }
            catch (Exception ex)
            {

                return new MSota.Responses.TransactionsResponse(new MSota.Responses.Error
                {
                    szErrorMessage = ex.Message,
                    szStackTrace = ex.StackTrace,
                    bErrorFound = true
                }, lsTransactions, lsFactions, HttpStatusCode.InternalServerError);
            }

        }

        public Responses.TransactionsResponse GetTransaction(TransactionModel tm)
        {
            throw new NotImplementedException();
        }
    }
}
