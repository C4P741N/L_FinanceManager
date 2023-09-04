namespace MSota.Models
{
    public class AccountLedgerModel
    {
        public decimal SumCreditAmount { get; set; }
        public decimal SumDepositAmount { get; set; }
        public List<QuotaSummaryModel> Quota { get; set; } = new List<QuotaSummaryModel>();
    }
}

