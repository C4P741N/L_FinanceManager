using System.ComponentModel.DataAnnotations;

namespace MSota.Models
{
    public class StatisticsModel
    {
        [Display(Name = "Amount Spent")]
        public double AmountSpent { get; set; }

        [Display(Name = "Amount Received")]
        public double AmountReceived { get; set; }

        [Display(Name = "Amount Borrowed")]
        public double AmountBorrowed { get; set; }

        [Display(Name = "Amount Charged")]
        public double AmountCharged { get; set; }
    }
}
