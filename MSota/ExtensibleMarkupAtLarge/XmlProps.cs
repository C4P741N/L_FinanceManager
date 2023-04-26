using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;

namespace MSota.ExtensibleMarkupAtLarge
{
    public class XmlProps : IXmlProps
    {
        private string xdateTime;
        public string szUniqueKey { get; set; }
        public string szProtocol { get; set; }
        public string szAddress { get; set; }
        public string szDate { get; set; }
        public string szType { get; set; }
        public string szSubject { get; set; }
        public string szBody { get; set; }
        public string szToa { get; set; }
        public string szSc_toa { get; set; }
        public string szService_center { get; set; }
        public string szRead { get; set; }
        public string szStatus { get; set; }
        public string szLocked { get; set; }
        public string szDate_sent { get; set; }
        public string szSub_id { get; set; }
        public string szReadable_date
        {
            get { 
                return Convert.ToString(DateTime.ParseExact(xdateTime, "MMM d, yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToUniversalTime()); 
            }
            set { 
                xdateTime = value; 
            }
        }
        public string szCode { get; set; }
        public string szPayBill_TillNo { get; set; }
        public double dCashAmount { get; set; }
        public string szRName { get; set; }
        public string szRPhoneNo { get; set; }
        public double dBalance { get; set; }
        //public string szRDate { get; set; }
        public string szRAccNo { get; set; }
        public string szTransactionCost { get; set; }
        public EnumsAtLarge.EnumContainer.TransactionQuota szQuota { get; set; }
        public double dFulizaLimit { get; set; }
        public double dFulizaBorrowed { get; set; }
        public double dCharges { get; set; }
        public double dFulizaAmount { get; set; }
    }
}
