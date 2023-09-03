using MSota.Models;
using System.Net;

namespace MSota.Responses
{
    public class TransactionsResponseII : BaseResponse
    {
        public AccountLegerModel _legerModel { get; set; }
        public TransactionsResponseII
            (Error error, AccountLegerModel legerModel, HttpStatusCode statusCode)
            : base(error, statusCode)
        {
            _legerModel = legerModel;
        }

        //public TransactionsResponseII
        //    (Error error, TransactionModel oModel, HttpStatusCode statusCode)
        //    : base(error, statusCode)
        //        => _transaction = oModel;
    }
}
