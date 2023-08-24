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

        public JsonSmsProps MessageExtractBegin(JsonBodyProps vals)
        {
            switch (vals.DocType)
            {
                case "KCB":
                    return _ExtractBodyForKCB(vals);
                case "MPESA":
                    return _ExtractBodyForMPESA(vals);
            }

            return vals.smsProps;
        }

        private JsonSmsProps _ExtractBodyForKCB(JsonBodyProps sms)
        {
            JsonSmsProps message = sms.smsProps;

            return message;
        }

        private JsonSmsProps _ExtractBodyForMPESA(JsonBodyProps sms)
        {
            string szBody = sms.Body;
            string[] status = new string[3];
            string[] moneyArray = new string[] { "", "", "", "" };
            string[] wordsArray = szBody.Split(' ');
            JsonSmsProps message = null;

            message = ProcessTransaction(szBody, ref status);

            if (message.Quota == EnumsAtLarge.EnumContainer.TransactionQuota.None ||
                message.Quota == EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction)
                return message;

            message.DocEntry = _fortmater.GetUniqueKey();

            message.TranId = wordsArray[0];
            message.Recepient = _fortmater.GlobalRNameGetter(szBody, status, message.Quota);
            message.AccNo = _fortmater.GlobalAccNoAndPhoneNoGetter(szBody);
            moneyArray = _fortmater.GlobalCashGetterArray(szBody);

            switch (message.Quota)
            {
                case EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit:

                    message.TranAmount = _fortmater.CashConverter(moneyArray[0]);
                    message.Balance = _fortmater.CashConverter(moneyArray[1]);
                    break;
                case EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount:
                case EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment:
                case EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer:
                case EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase:

                    if(moneyArray.Count() > 2)
                        message.Charges = -_fortmater.CashConverter(moneyArray[2]);
                    
                    message.TranAmount = -_fortmater.CashConverter(moneyArray[0]);
                    message.Balance = _fortmater.CashConverter(moneyArray[1]);
                    break;
                case EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit:

                    message.TranAmount = _fortmater.CashConverter(moneyArray[0]);
                    message.Charges = -_fortmater.CashConverter(moneyArray[1]);
                    message.Balance = _fortmater.CashConverter(moneyArray[2]);
                    break;
                case EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit:

                    message.TranAmount = -_fortmater.CashConverter(moneyArray[0]);
                    message.Balance = _fortmater.CashConverter(moneyArray[2]);
                    break;
            }

            return message;
        }

        public JsonSmsProps ProcessTransaction(string szBody,
                                            ref string[] srtatus)
        {
            JsonSmsProps message = new JsonSmsProps();
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
                {"pay your outstanding Fuliza",                                 (EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit,           new string[3] {"Confirmed", "from", "outstanding"})},
                {"Reversal of transaction",                                     (EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit,       new string[3])},
            };

            foreach (var keywordMapping in keywordMappings)
            {
                if (szBody.Contains(keywordMapping.Key))
                {
                    message.Quota = keywordMapping.Value.Item1;
                    status = keywordMapping.Value.Item2;
                    break;
                }
            }
            srtatus = status;
            message.Recepient = message.Quota.ToString();
            return message;
        }
    }
}
