using MSota.DataLibrary;
using MSota.Models;

namespace MSota.Responses
{
    public class TransactionsResponse : BaseResponse
    {
         public List<TransactionModel> Value { get; set; }
        public TransactionModel Transaction { get; set; }
        public TransactionsResponse
            (Error error, List<TransactionModel> lsModel) 
            : base(!error.bErrorFound,error) 
                => Value = lsModel;

        public TransactionsResponse
            (Error error, TransactionModel oModel) 
            : base(!error.bErrorFound, error) 
                => Transaction = oModel;
    }
}
