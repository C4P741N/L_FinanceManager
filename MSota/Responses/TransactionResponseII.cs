using MSota.Models;
using Newtonsoft.Json;
using System.Net;

namespace MSota.Responses
{
    public class TransactionsResponseII : BaseResponse, IJSONConverters
    {
        public AccountLedgerModel _ledgerModel { get; set; }

        public string AccountLedgerSerializedJSON()
        {
            return JsonConvert.SerializeObject(_ledgerModel);
        }

        public Calendar_II AccountLedgerDeserializedJSON(string jsonString)
        {
            throw new NotImplementedException();
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
