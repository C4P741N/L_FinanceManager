using MSota.BaseFormaters;
using MSota.ExtensibleMarkupAtLarge;
using MSota.JavaScriptObjectNotation;
using System.Diagnostics;

namespace MSota.Extractors
{
    public class Ex_SMS : IEx_SMS
    {
        //private Z_Formaters.Formaters _fortmater = null;
        IFortmaterAtLarge _fortmater;

        public Ex_SMS(IFortmaterAtLarge fortmater)
        {
            _fortmater = fortmater;
        }

        public SmsProps MessageExtractBegin(string szKey, Values sms)
        {
            switch (szKey)
            {
                case "KCB":
                    return _ExtractBodyForKCB(sms);
                case "MPESA":
                    return _ExtractBodyForMPESA(sms);
            }

            return sms.smsProps;
        }

        private SmsProps _ExtractBodyForKCB(Values sms)
        {
            string szBody = sms.message;

            SmsProps message = sms.smsProps;

            while (true)
            {
                message.dCashAmount = _fortmater.GlobalCashGetter(szBody);
                message.szCode = _fortmater.GetMpesaCode(szBody);

                if (szBody.Contains("sent to"))
                {
                    message.szRAccNo = _fortmater.GetAccountNumber(szBody);
                    message.szRName = _fortmater.RNameCreatorFromAccNo(message.szRAccNo);
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount;

                    if (szBody.Contains("Pay Bill"))
                    {
                        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.PayBillPayment;
                        message.szPayBill_TillNo = _fortmater.GetPayBillNo(szBody);
                    }

                    break;
                }
                if (szBody.Contains("has transfered"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit;
                    message.szRName = _fortmater.GetKCBContactName(szBody);
                }

                break;
            }

            return message;
        }

        private SmsProps _ExtractBodyForMPESA(Values sms)
        {
            string szBody = sms.message;
            string[] status = new string[3];
            string[] moneyArray = new string[] { "", "", "", "" };
            string[] wordsArray = szBody.Split(' ');
            SmsProps message = null;

            message = ProcessTransaction(szBody, ref status);

            if (message.szQuota == EnumsAtLarge.EnumContainer.TransactionQuota.None ||
                message.szQuota == EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction)
                return message;

            message.szCode = wordsArray[0];
            message.szRName = _fortmater.GlobalRNameGetter(szBody, status, message.szQuota);
            message.szRAccNo = _fortmater.GlobalAccNoAndPhoneNoGetter(szBody);
            moneyArray = _fortmater.GlobalCashGetterArray(szBody);

            if (string.IsNullOrEmpty(message.szRName))
                message.szRName = "Null";

            switch (message.szQuota)
            {
                case EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit:
                case EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount:
                case EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment:
                case EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer:
                case EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase:

                    if(moneyArray.Count() > 2)
                        message.dCharges = _fortmater.CashConverter(moneyArray[2]);
                    
                    message.dCashAmount = _fortmater.CashConverter(moneyArray[0]);
                    message.dBalance = _fortmater.CashConverter(moneyArray[1]);
                    break;
                case EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit:
                    message.dFulizaBorrowed = _fortmater.CashConverter(moneyArray[0]);
                    message.dCharges = _fortmater.CashConverter(moneyArray[1]);
                    message.dFulizaAmount = _fortmater.CashConverter(moneyArray[2]);
                    break;
                case EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit:
                    message.dFulizaBorrowed = _fortmater.CashConverter(moneyArray[0]);
                    message.dBalance = _fortmater.CashConverter(moneyArray[2]);
                    break;
            }

            return message;
        }

        public SmsProps ProcessTransaction(string szBody,
                                            ref string[] srtatus)
        {
            SmsProps message = new SmsProps();
            string[] status = new string[3];

            Dictionary<string, (EnumsAtLarge.EnumContainer.TransactionQuota, string[])> keywordMappings = new Dictionary<string, (EnumsAtLarge.EnumContainer.TransactionQuota, string[])>
            {
                {"Failed",                                                      (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"balance was",                                                 (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"currently underway",                                          (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"cancelled the transaction",                                   (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"failed",                                                      (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"your Fuliza M-PESA limit is",                                 (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"The number you are trying to pay has not joined the service", (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"not able to process your request",                            (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,   new string[0])},
                {"received",                                                    (EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit,       new string[2] {"", "from"})},
                {"Give",                                                        (EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit,       new string[2] {"", "from"})},
                {"airtime",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,      new string[2] {"", "for"})},
                {"Safaricom Limited",                                           (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,      new string[2] {"", "for"})},
                {"Safaricom Offers",                                            (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,      new string[2] {"", "for"})},
                {"paid to",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment,      new string[2] {"paid", "to"})},
                {"sent to",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer,     new string[2] {"sent", "to"})},
                {"EASYFLOAT",                                                   (EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer,     new string[2] {"sent", "to"})},
                {"Paybill",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.PayBillPayment,       new string[2] {"sent", "to"})},
                {"PMWithdraw",                                                  (EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount,      new string[3] {"", "from", "withdraw"})},
                {"AMWithdraw",                                                  (EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount,      new string[3] {"", "from", "withdraw"})},
                {"Interest charged",                                            (EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit,            new string[3] {"Confirmed", "from", "outstanding"})},
                {"to fully pay your outstanding Fuliza",                        (EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit,           new string[3] {"Confirmed", "from", "outstanding"})},
            };

            foreach (var keywordMapping in keywordMappings)
            {
                if (szBody.Contains(keywordMapping.Key))
                {
                    message.szQuota = keywordMapping.Value.Item1;
                    status = keywordMapping.Value.Item2;
                    break;
                }
            }
            srtatus = status;
            message.szRName = message.szQuota.ToString();
            return message;
        }
    }
}
