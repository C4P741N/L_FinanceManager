//using MSota.DataLibrary;
//using MSota.Models;
//using System.Transactions;

//namespace MSota.Base
//{
//    public class BaseCommands
//    {
//        ITransactionsResponse _transactions;
//        BaseCommands _baseCommands;


//        public BaseCommands(ITransactionsResponse transactions)
//        {
//            _transactions = transactions;
//            //_baseCommands = baseCommands;
//        }

//        public static void BeginDataInsertIf()
//        {
//        }

//        public ITransactionsResponse Transactions()
//        {
//            //BaseCommands baseCommands = new BaseCommands(_transactions);
//            //_baseCommands = new BaseCommands(_transactions);
//            //TransactionsResponse transactions = new TransactionsResponse();

//            _transactions.CollectTransactions();

//            return null;
//        }
//    }
//}
