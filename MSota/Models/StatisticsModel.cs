using System.ComponentModel.DataAnnotations;

namespace MSota.Models
{
    public class StatisticsModel
    {
        [Display(Name = "Spent")]
        public double AmountSpent { get; set; }

        [Display(Name = "Received")]
        public double AmountReceived { get; set; }

        [Display(Name = "Borrowed")]
        public double AmountBorrowed { get; set; }

        [Display(Name = "Charged")]
        public double AmountCharged { get; set; }

        [Display(Name = "Receipient Names")]
        public string? ListReceipientNames { get; set; }

        [Display(Name = "Amount Spent On Receipient")]
        public double ListAmountSpentOnReceipient { get; set; }

        [Display(Name = "Date Amount WasSpent On Receipient")]
        public double ListDateAmountWasSpentOnReceipient { get; set; }
    }
}
