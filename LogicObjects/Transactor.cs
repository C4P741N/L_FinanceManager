using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicObjects
{
    public class Transactor
    {
        private ICollection<L_Recepients> collRecepient;

        public Transactor()
        {
            collRecepient = new HashSet<L_Recepients>();
        }
        public double TotAlamountTransacted { get; set; }

        public double TotalAmountDeposited { get; set; }

        public double TotalLoanBorrowed { get; set; }

        public int TransactionsCount { get; set; }

        ////public double TotalFulizaBorrowed { get; set; }

        //public L_Recepients AddRecepients(L_Recepients rps)
        //{
        //    collRecepient.Add(rps);

        //    return rps;
        //}

        public L_Recepients recepients { get; set; } = new L_Recepients();
    }
}
