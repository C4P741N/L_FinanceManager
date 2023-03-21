namespace MSota.Models
{
    public class TransactionModel
    {
        public string? TransactionID { get; set; }

        public DateTime TranactionDate { get; set; }

        public double TransactionAmount { get; set; }

        public double TranactionCharge { get; set; }

        public string? TranactionQuota { get; set; }

        public double LoanBorrowed { get; set; }

        //public double FulizaCharge { get; set; }

        public double LoanBalance { get; set; }
    }
}
