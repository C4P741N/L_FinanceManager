﻿using MSota.Models;

namespace MSota.Accounts
{
    public class Transactions : ITransactions
    {
        public Responses.TransactionsResponse GetAllTransactions()
        {
            List<TransactionModel> lsTransactions = new List<TransactionModel>();

            try
            {

                return new Responses.TransactionsResponse(null, lsTransactions);

            }
            catch (Exception ex)
            {

                return new MSota.Responses.TransactionsResponse(new MSota.Responses.Error
                {
                    szErrorMessage = ex.Message,
                    bErrorFound = true
                }, lsTransactions);
            }

        }

        public Responses.TransactionsResponse GetTransaction(TransactionModel tm)
        {
            throw new NotImplementedException();
        }
    }
}
