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
        IJSONConverters _jSONConverters;
        List<TransactionModel> lsTransactions = null;
        List<FactionsModel> lsFactions = null;
        
        public Transactions(ISqlDataServer sqlDataServer, IJSONConverters jSONConverters)
        {
            _sqlDataServer = sqlDataServer;
            _jSONConverters = jSONConverters;
        }

        public Responses.TransactionsResponseII GetAllTransactionsII(string dateRangeJson)
        {

            try
            {
                Calendar cals = null;

                Calendar_II cal = _jSONConverters.AccountLedgerDeserializedJSON(dateRangeJson);

                var test = cal.dt_ToDate;


                AccountLedgerModel accLeger = _sqlDataServer.LoadAccountLegerSummary(cal);

                if (accLeger == null)
                    return new Responses.TransactionsResponseII(new MSota.Responses.Error { bErrorFound = true }, new AccountLedgerModel(), System.Net.HttpStatusCode.NoContent);

                accLeger.Quota = _sqlDataServer.LoadAccountQuotaSummary(cal);

                return new Responses.TransactionsResponseII(new MSota.Responses.Error(), accLeger, System.Net.HttpStatusCode.Accepted);

            }
            catch (Exception ex)
            {

                return new MSota.Responses.TransactionsResponseII(
                    new MSota.Responses.Error
                    {
                        szErrorMessage = ex.Message,
                        szStackTrace = ex.StackTrace,
                        bErrorFound = true
                    },
                    new AccountLedgerModel(),
                    HttpStatusCode.InternalServerError);
            }

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

                return new MSota.Responses.TransactionsResponse(
                    new MSota.Responses.Error
                    {
                        szErrorMessage = ex.Message,
                        szStackTrace = ex.StackTrace,
                        bErrorFound = true
                    }, 
                    lsTransactions, 
                    lsFactions, 
                    HttpStatusCode.InternalServerError);
            }

        }

        public Responses.TransactionsResponse GetTransaction(TransactionModel tm)
        {
            throw new NotImplementedException();
        }
    }
}
