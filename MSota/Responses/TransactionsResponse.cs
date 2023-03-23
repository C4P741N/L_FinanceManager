using MSota.DataLibrary;
using MSota.Models;

namespace MSota.Responses
{
    public class TransactionsResponse : BaseResponse
    {
        public List<TransactionModel> _transactions { get; set; }
        public TransactionModel _transaction { get; set; }
        public List<FactionsModel> _factions { get; set; }
        public TransactionsResponse
            (Error error, List<TransactionModel> lsTransactions, List<FactionsModel> lsFactions)
            : base(error)
        {
            _transactions = lsTransactions; 
            _factions = lsFactions;
        }

        public TransactionsResponse
            (Error error, TransactionModel oModel) 
            : base(error) 
                => _transaction = oModel;
    }
}
