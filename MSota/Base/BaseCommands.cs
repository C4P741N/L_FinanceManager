using MSota.Models;

namespace MSota.Base
{
    public class BaseCommands
    {
        public static void BeginDataInsertIf()
        {
        }

        public static TransactionsResponse Transactions()
        {
            return new TransactionsResponse();
        }
    }
}
