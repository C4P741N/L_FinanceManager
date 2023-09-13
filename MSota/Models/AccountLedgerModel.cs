using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;

namespace MSota.Models
{
    public class AccountLedgerModel
    {
        private bool xIsDoubleEntryValid = false;

        public decimal SumCreditAmount { get; set; }
        public decimal SumDepositAmount { get; set; }
        public bool IsDoubleEntryValid
        {
            get { return xIsDoubleEntryValid; }
            set
            {
                xIsDoubleEntryValid = SumCreditAmount != 0 && SumDepositAmount != 0;
            }
        }

        public List<QuotaSummaryModel> Quota { get; set; } = new List<QuotaSummaryModel>();
    }
}

