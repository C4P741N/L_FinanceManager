using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicObjects
{
    public class L_Transaction
    {
        public string? TransactionID { get; set; }

        public long TranactionDate { get; set; }

        public double TransactionAmount { get; set; }

        public double TranactionCharge { get; set; }

        public string? TranactionQuota { get; set; }

        public double LoanBorrowed { get; set; }

        //public double FulizaCharge { get; set; }

        public double LoanBalance { get; set; }



        
    }
}
