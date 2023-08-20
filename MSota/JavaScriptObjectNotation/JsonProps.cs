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

        public List<Values> Values { get; set; } = new List<Values>();
    }

    public class Values
    {
        public string ?message { get; set; }
        public string ?sender { get; set; }
        public DateTime date { get; set; }
        public string read { get; set; }
        public int type { get; set; }
        public int thread { get; set; }
        public string ?service { get; set; }
        public SmsProps smsProps { get; set; } = new SmsProps();
    }

    public class SmsProps
    {
        double dBalance { get; set; }
        double dCashAmount { get; set; }
        double dCharges { get; set; }
        double dFulizaAmount { get; set; }
        double dFulizaBorrowed { get; set; }
        double dFulizaLimit { get; set; }
        string szAddress { get; set; }
        string szBody { get; set; }
        string szCode { get; set; }
        string szDate { get; set; }
        string szDate_sent { get; set; }
        string szLocked { get; set; }
        string szPayBill_TillNo { get; set; }
        string szProtocol { get; set; }
        EnumsAtLarge.EnumContainer.TransactionQuota szQuota { get; set; }
        string szRAccNo { get; set; }
        string szRead { get; set; }
        string szReadable_date { get; set; }
        string szRName { get; set; }
        string szRPhoneNo { get; set; }
        string szSc_toa { get; set; }
        string szService_center { get; set; }
        string szStatus { get; set; }
        string szSub_id { get; set; }
        string szSubject { get; set; }
        string szToa { get; set; }
        string szTransactionCost { get; set; }
        string szType { get; set; }
        string szUniqueKey { get; set; }
    }
}
