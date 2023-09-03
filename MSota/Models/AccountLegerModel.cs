namespace MSota.Models
{
    public class AccountLegerModel
    {
        public decimal SumCreditAmount { get; set; }
        public decimal SumDepositAmount { get; set; }
        //public List<QuotaSummaryModel> Quota { get; set; } = new List<QuotaSummaryModel>();

        public string Test { get; set; } = "Test message from serve";
    }
}

