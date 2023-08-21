using MSota.Controllers;
using System.Globalization;
using System;
using System.Data;

namespace MSota.JavaScriptObjectNotation
{
    //public class JsonProps : IJsonProps
    //{
    //    public List<smsMessages> Properties { get; set; } = new List<smsMessages>();

    //    public Dictionary<string, List<smsMessages>>? value { get; set; }
    //}

    

    public class SMSMessages
    {
        public string ?Key { get; set; }

        public Values value { get; set; } = new Values();

        public List<Values> values { get; set; } = new List<Values>();
    }

    public class Values
    {
        public string ?message { get; set; }
        public string ?sender { get; set; }
        public DateTime readableDate { get; set; }
        public long lDate { get; set; }
        public string read { get; set; }
        public int type { get; set; }
        public int thread { get; set; }
        public string ?service { get; set; }
        public SmsProps smsProps { get; set; }

    }

    public class SmsProps
    {
        public double dBalance { get; set; }
        public double dCashAmount { get; set; }
        public double dCharges { get; set; }
        public double dFulizaAmount { get; set; }
        public double dFulizaBorrowed { get; set; }
        public double dFulizaLimit { get; set; }
        public string szAddress { get; set; }
        public string szBody { get; set; }
        public string szCode { get; set; }
        public string szDate { get; set; }
        public string szDate_sent { get; set; }
        public string szLocked { get; set; }
        public string szPayBill_TillNo { get; set; }
        public string szProtocol { get; set; }
        public EnumsAtLarge.EnumContainer.TransactionQuota szQuota { get; set; }
        public string szRAccNo { get; set; }
        public string szRead { get; set; }
        public string szReadable_date { get; set; }
        public string szRName { get; set; }
        public string szRPhoneNo { get; set; }
        public string szSc_toa { get; set; }
        public string szService_center { get; set; }
        public string szStatus { get; set; }
        public string szSub_id { get; set; }
        public string szSubject { get; set; }
        public string szToa { get; set; }
        public string szTransactionCost { get; set; }
        public string szType { get; set; }
        public string szUniqueKey { get; set; }
    }
}
