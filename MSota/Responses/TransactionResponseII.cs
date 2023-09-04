using MSota.Models;
using Newtonsoft.Json;
using System.Net;

namespace MSota.Responses
{
    public class TransactionsResponseII : BaseResponse
    {
        public AccountLedgerModel _ledgerModel { get; set; }

        public string AccountLedgerJSON()
        {
            return JsonConvert.SerializeObject(_ledgerModel);
        }
        public TransactionsResponseII
            (Error error, AccountLedgerModel ledgerModel, HttpStatusCode statusCode)
            : base(error, statusCode)
        {
            _ledgerModel = ledgerModel;
        }

        //public TransactionsResponseII
        //    (Error error, TransactionModel oModel, HttpStatusCode statusCode)
        //    : base(error, statusCode)
        //        => _transaction = oModel;
    }
}
