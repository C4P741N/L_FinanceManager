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

        public string TranactionQuota { get; set; }

        public double FulizaBorrowed { get; set; }

        public double FulizaPaid { get; set; }

        public double FulizaDebtBalance { get; set; }
    }
}
