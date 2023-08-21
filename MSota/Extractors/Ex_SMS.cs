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

            SmsProps message = new SmsProps();

            //string szBody = message.szBody;
            string szID = string.Empty;
            bool bWrongData = false;
            bool bFuliza = false;
            string[] status = new string[3];
            string[] moneyArray = new string[] { "", "", "", "" };

            string[] wordsArray = szBody.Split(' ');

            while (true)
            {
                //xxGetFilteringParams(sms);
                ProcessTransaction(szBody);


                if (szBody.Contains("Failed") ||
                    szBody.Contains("balance was") ||
                    szBody.Contains("currently underway") ||
                    szBody.Contains("cancelled the transaction") ||
                    szBody.Contains("failed") ||
                    szBody.Contains("your Fuliza M-PESA limit is") ||
                    szBody.Contains("The number you are trying to pay has not joined the service") ||
                    szBody.Contains("not able to process your request"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction;

                    return message;
                }
                if (szBody.Contains("received") ||
                    szBody.Contains("Give"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit;

                    szID = "received";

                    status[0] = "";
                    status[1] = "from";
                }
                if (szBody.Contains("airtime") ||
                    szBody.Contains("Safaricom Limited") == true ||
                    szBody.Contains("Safaricom Offers") == true)
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase;

                    szID = "airtime";

                    if (szBody.Contains("airtime for"))
                    {
                        status[0] = "";
                        status[1] = "for";
                    }
                }
                if (szBody.Contains("paid to"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment;
                    szID = "paid";

                    status[0] = "paid";
                    status[1] = "to";
                }
                if (szBody.Contains("sent to") ||
                    szBody.Contains("EASYFLOAT") ||
                    szBody.Contains("Paybill"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer;

                    if (szBody.Contains("Paybill"))
                        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.PayBillPayment;

                    szID = "sent";

                    status[0] = "sent";
                    status[1] = "to";
                }
                if (szBody.Contains("PMWithdraw") ||
                    szBody.Contains("AMWithdraw"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount;
                    szID = "withdraw";

                    status[0] = "";
                    status[1] = "from";
                    status[2] = szID;
                }
                if (szBody.Contains("Fuliza"))
                {
                    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit;
                    szID = "borrowed";

                    status[0] = "Confirmed";
                    status[1] = "from";
                    status[2] = "outstanding";

                    if (szBody.Contains("fully pay your"))
                    {
                        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit;
                    }
                    message.szRName = message.szQuota.ToString();

                }

                if (message.szQuota == EnumsAtLarge.EnumContainer.TransactionQuota.None)
                    break;

                message.szCode = wordsArray[0];

                if (string.IsNullOrEmpty(message.szRName))
                    message.szRName = _fortmater.GlobalRNameGetter(szBody, status);

                message.szRAccNo = _fortmater.GlobalAccNoAndPhoneNoGetter(szBody);
                moneyArray = _fortmater.GlobalCashGetterArray(szBody);

                if (string.IsNullOrEmpty(message.szRName))
                    message.szRName = "Null";

                message.dCashAmount = _fortmater.CashConverter(moneyArray[0]);
                message.dBalance = _fortmater.CashConverter(moneyArray[1]);

                if (moneyArray.Count() >= 3)
                    message.dCharges = _fortmater.CashConverter(moneyArray[2]);

                if (message.dCharges > 60)
                {

                }

                switch (message.szQuota)
                {
                    case EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount:
                        message.szRName = _fortmater.GlobalRNameGetter(szBody, status);
                        break;
                    case EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment:
                    case EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer:
                    case EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit:
                        break;
                    case EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase:
                        message.szRName = message.szQuota.ToString();
                        break;
                        //case EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit:
                        //    message.szRName = message.szQuota.ToString();
                        //    message.dBalance = _fortmater.CashConverter(moneyArray[2]);

                        //    if (szBody.Contains("charged"))
                        //    {
                        //        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit;
                        //        message.szRName = message.szQuota.ToString();
                        //    }

                        //    break;
                }
                break;
            }
            return message;
        }

        public SmsProps ProcessTransaction(string szBody)
        {
            SmsProps message = new SmsProps();
            string szID = "";
            string[] status = new string[3];

            Dictionary<string, (EnumsAtLarge.EnumContainer.TransactionQuota, string, string[])> keywordMappings = new Dictionary<string, (EnumsAtLarge.EnumContainer.TransactionQuota, string, string[])>
            {
                {"Failed",                                                      (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"balance was",                                                 (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"currently underway",                                          (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"cancelled the transaction",                                   (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"failed",                                                      (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"your Fuliza M-PESA limit is",                                 (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"The number you are trying to pay has not joined the service", (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"not able to process your request",                            (EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction,    "",             new string[0])},
                {"received",                                                    (EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit,        "received",     new string[2] {"", "from"})},
                {"Give",                                                        (EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit,        "received",     new string[2] {"", "from"})},
                {"airtime",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,       "airtime",      new string[2] {"", "for"})},
                {"Safaricom Limited",                                           (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,       "airtime",      new string[2] {"", "for"})},
                {"Safaricom Offers",                                            (EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase,       "airtime",      new string[2] {"", "for"})},
                {"paid to",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment,       "paid",         new string[2] {"paid", "to"})},
                {"sent to",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer,      "sent",         new string[2] {"sent", "to"})},
                {"EASYFLOAT",                                                   (EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer,      "sent",         new string[2] {"sent", "to"})},
                {"Paybill",                                                     (EnumsAtLarge.EnumContainer.TransactionQuota.PayBillPayment,        "sent",         new string[2] {"sent", "to"})},
                {"PMWithdraw",                                                  (EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount,       "withdraw",     new string[3] {"", "from", "withdraw"})},
                {"AMWithdraw",                                                  (EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount,       "withdraw",     new string[3] {"", "from", "withdraw"})},
                {"Fuliza",                                                      (EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit,             "borrowed",     new string[3] {"Confirmed", "from", "outstanding"})},
            };

            foreach (var keywordMapping in keywordMappings)
            {
                if (szBody.Contains(keywordMapping.Key))
                {
                    message.szQuota = keywordMapping.Value.Item1;
                    szID = keywordMapping.Value.Item2;
                    status = keywordMapping.Value.Item3;
                    break;
                }
            }

            message.szRName = message.szQuota.ToString();
            return message;
        }

        private SmsProps xxGetFilteringParams(Values sms)
        {
            string szBody = sms.message;
            SmsProps message = new SmsProps();

            //switch (szBody)
            //{
            //    case szBody.Contains("received"):
            //        break;
            //}

            //if (szBody.Contains("Failed") ||
            //    szBody.Contains("balance was") ||
            //    szBody.Contains("currently underway") ||
            //    szBody.Contains("cancelled the transaction") ||
            //    szBody.Contains("failed") ||
            //    szBody.Contains("your Fuliza M-PESA limit is") ||
            //    szBody.Contains("The number you are trying to pay has not joined the service") ||
            //    szBody.Contains("not able to process your request"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.InvalidTransaction;

            //    return message;
            //}
            //if (szBody.Contains("received") ||
            //    szBody.Contains("Give"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit;

            //    szID = "received";

            //    status[0] = "";
            //    status[1] = "from";
            //}
            //if (szBody.Contains("airtime") ||
            //    szBody.Contains("Safaricom Limited") == true ||
            //    szBody.Contains("Safaricom Offers") == true)
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase;

            //    szID = "airtime";

            //    if (szBody.Contains("airtime for"))
            //    {
            //        status[0] = "";
            //        status[1] = "for";
            //    }
            //}
            //if (szBody.Contains("paid to"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment;
            //    szID = "paid";

            //    status[0] = "paid";
            //    status[1] = "to";
            //}
            //if (szBody.Contains("sent to") ||
            //    szBody.Contains("EASYFLOAT") ||
            //    szBody.Contains("Paybill"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer;

            //    if (szBody.Contains("Paybill"))
            //        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.PayBillPayment;

            //    szID = "sent";

            //    status[0] = "sent";
            //    status[1] = "to";
            //}
            //if (szBody.Contains("PMWithdraw") ||
            //    szBody.Contains("AMWithdraw"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount;
            //    szID = "withdraw";

            //    status[0] = "";
            //    status[1] = "from";
            //    status[2] = szID;
            //}
            //if (szBody.Contains("Fuliza"))
            //{
            //    message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit;
            //    szID = "borrowed";

            //    status[0] = "Confirmed";
            //    status[1] = "from";
            //    status[2] = "outstanding";

            //    if (szBody.Contains("fully pay your"))
            //    {
            //        message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit;
            //    }
            //    message.szRName = message.szQuota.ToString();

            //}

            return message;
        }
    }
}
