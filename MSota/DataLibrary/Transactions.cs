using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;

namespace MSota.DataLibrary
{
    public class Transactions : IEnumerable<Transaction>, ITransactions
    {
        private ICollection<Transaction> collTransaction;
        
        public Transactions()
        {
            collTransaction = new HashSet<Transaction>();
            //dbAccessor = new DbAccessor();
        }

        public IEnumerator<Transaction> GetEnumerator()
        {
            return collTransaction.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collTransaction.GetEnumerator();
        }

        //public int TransactionCount { get { return collTransaction.Count; } }

        public Transaction AddTransaction(Transaction tr)
        {
            collTransaction.Add(tr);

            return tr;
        }

        public Transaction RemoveTransaction(Transaction tr)
        {
            collTransaction.Remove(tr);

            return tr;
        }

        public bool TransactionExist(string szTransactionID)
        {
            Transaction tr = collTransaction.FirstOrDefault(tr => tr.TransactionID == szTransactionID);

            if (tr == null)
            {
                return false;
            }

            return true;
        }

        public Transaction GetTransaction(string szTransactionID)
        {
            Transaction tr = collTransaction.FirstOrDefault(tr => tr.TransactionID == szTransactionID);

            return tr;
        }

        
    }
}
