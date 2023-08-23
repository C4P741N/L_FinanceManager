using MSota.Controllers;
using System.Globalization;
using System;
using System.Data;
using System.Threading;

namespace MSota.JavaScriptObjectNotation
{
    public class JsonSmsProps
    {
        public string? DocEntry { get; set; }
        public string? TranId { get; set; }
		public string? Recepient { get; set; }
        public string? AccNo { get; set; }
		public double TranAmount { get; set; }
        public double Balance { get; set; }
        public double Charges { get; set; }
        public EnumsAtLarge.EnumContainer.TransactionQuota Quota { get; set; } = EnumsAtLarge.EnumContainer.TransactionQuota.None;
    }
}
