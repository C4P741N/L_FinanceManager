using MSota.Controllers;
using System.Globalization;
using System;
using System.Data;
using System.Threading;

namespace MSota.JavaScriptObjectNotation
{
    public class JsonSmsProps
    {
        public string? DocEntry { get; set; } = "0";
        public string? TranId { get; set; } = "0";
		public string? Recepient { get; set; } = "0";
        public string? AccNo { get; set; } = "0";
        public double TranAmount { get; set; }
        public double Balance { get; set; }
        public double Charges { get; set; }
        public EnumsAtLarge.EnumContainer.TransactionQuota Quota { get; set; } = EnumsAtLarge.EnumContainer.TransactionQuota.None;
        public EnumsAtLarge.EnumContainer.TransactionType TranType { get; set; } = EnumsAtLarge.EnumContainer.TransactionType.None;
    }
}
