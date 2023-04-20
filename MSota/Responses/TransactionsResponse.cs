using MSota.DataLibrary;
using MSota.Models;
using System.Net;

namespace MSota.Responses
{
    public class TransactionsResponse : BaseResponse
    {
        public List<TransactionModel> _transactions { get; set; }
        public TransactionModel _transaction { get; set; }
        public List<FactionsModel> _factions { get; set; }
        public TransactionsResponse
            (Error error, List<TransactionModel> lsTransactions, List<FactionsModel> lsFactions, HttpStatusCode statusCode)
            : base(error, statusCode)
        {
            _transactions = lsTransactions; 
            _factions = lsFactions;
        }

        public TransactionsResponse
            (Error error, TransactionModel oModel, HttpStatusCode statusCode) 
            : base(error, statusCode) 
                => _transaction = oModel;
    }
}
