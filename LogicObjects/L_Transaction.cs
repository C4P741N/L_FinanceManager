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

        public double TranactionCost { get; set; }

        public string? TranactionQuota { get; set; }

        public double FulizaBorrowed { get; set; }

        public double FulizaCharge { get; set; }

        public double FulizaDebtBalance { get; set; }



        public double TotalTransactionDeposited { get; set; }

        public double TotalTransactionWithdrawn { get; set; }

        public double TotalTranactionCost { get; set; }
                      
        public double TotalFulizaBorrowed { get; set; }
                      
        public double TotalFulizaCharge { get; set; }
                      
        public double TotalFulizaDebtBalance { get; set; }
    }
}
