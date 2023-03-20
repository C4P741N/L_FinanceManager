using MSota.DataLibrary;
using MSota.Models;

namespace MSota.Responses
{
    public class TransactionsResponse : BaseResponse
    {
         public List<TransactionModel> _value { get; set; }
        public TransactionModel _transaction { get; set; }
        public TransactionsResponse
            (Error error, List<TransactionModel> lsModel)
            : base(error) => _value = lsModel;

        public TransactionsResponse
            (Error error, TransactionModel oModel) 
            : base(error) 
                => _transaction = oModel;
    }
}
