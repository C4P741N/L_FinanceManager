using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicObjects
{
    public class L_Recepient
    {
        public string RecepientName { get; set; } = string.Empty;    

        public string RecepientId { get; set; } = string.Empty;

        //public List<RecipientNUmber> RecieptNumber { get; set; } = new List<RecipientNUmber>();

        public string RecepientAccNo { get; set; } = string.Empty;

        public long Relationship { get; set; }

        public double TotalTransactionDeposited { get; set; }

        public double TotalTransactionWithdrawn { get; set; }

        public double TotalTranactionCost { get; set; }

        public L_Transactions transactions { get; set; } = new L_Transactions();

    }
}
