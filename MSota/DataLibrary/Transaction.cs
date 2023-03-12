namespace MSota.DataLibrary
{
    public class Transaction
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
