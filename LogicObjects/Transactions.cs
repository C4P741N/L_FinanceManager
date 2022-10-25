using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicObjects
{
    public class Transactions : IEnumerable<L_Transaction>
    {
        private ICollection<L_Transaction> collTransaction;
        public Transactions()
        {
            collTransaction = new HashSet<L_Transaction>();
        }
        public IEnumerator<L_Transaction> GetEnumerator()
        {
            return collTransaction.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collTransaction.GetEnumerator();
        }

        //public int TransactionCount { get { return collTransaction.Count; } }

        public L_Transaction AddTransaction(L_Transaction tr)
        {
            collTransaction.Add(tr);

            return tr;
        }

        public bool TransactionExist(string szTransactionID)
        {
            L_Transaction tr = collTransaction.FirstOrDefault(tr => tr.TransactionID == szTransactionID);

            if (tr == null)
            {
                return false;
            }

            return true;
        }

        public L_Transaction GetTransaction(string szTransactionID)
        {
            L_Transaction tr = collTransaction.FirstOrDefault(tr => tr.TransactionID == szTransactionID);

            return tr;
        }
    }
}
