using Microsoft.IdentityModel.Tokens;
using MSota.BaseFormaters;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MSota.ExtensibleMarkupAtLarge
{
    public class XmlDataFotmater : IXmlDataFotmater
    {
        //private Z_Formaters.Formaters _fortmater = null;
        IFortmaterAtLarge _fortmater;

        public XmlDataFotmater(IFortmaterAtLarge fortmater)
        {
            _fortmater = fortmater;
        }           
        public void BeginExtractKcbData(IXmlProps message)
        {
            _ExtractBodyForKCB(message);
        }
        private IXmlProps _ExtractBodyForKCB(IXmlProps message)
        {
            string szBody = message.szBody;

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
        public void BeginExtractMpesaData(IXmlProps message)
        {
            _ExtractBodyForMPESA(message);
        }

        private IXmlProps _ExtractBodyForMPESA(IXmlProps message)
        {
            string szBody = message.szBody;
            string szID = string.Empty;
            bool bWrongData = false;
            bool bFuliza = false;
            string[] status = new string[3];
            string[] moneyArray = new string[] {"", "", "",""};

            string[] wordsArray = szBody.Split(' ');

            while (true)
            {
                if (szBody.Contains("Failed") ||
                    szBody.Contains("balance was") ||
                    szBody.Contains("currently underway") ||
                    szBody.Contains("cancelled the transaction") ||
                    szBody.Contains("failed"))
                {
                    break;
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

                    if(szBody.Contains("airtime for"))
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

                    if(szBody.Contains("Paybill"))
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
                }

                if (message.szQuota == EnumsAtLarge.EnumContainer.TransactionQuota.None)
                    break;

                message.szCode = wordsArray[0];
                message.szRName = _fortmater.GlobalRNameGetter(szBody, status, message.szQuota);
                message.szRAccNo = _fortmater.GlobalAccNoAndPhoneNoGetter(szBody);
                moneyArray = _fortmater.GlobalCashGetterArray(szBody);

                if (string.IsNullOrEmpty(message.szRName))
                    message.szRName = "Null";

                message.dCashAmount = _fortmater.CashConverter(moneyArray[0]);
                message.dBalance = _fortmater.CashConverter(moneyArray[1]);
                if(moneyArray.Count() == 3)
                    message.dCharges = _fortmater.CashConverter(moneyArray[2]);

                switch (message.szQuota)
                {
                    case EnumsAtLarge.EnumContainer.TransactionQuota.WithdrawnAmount:
                        message.szRName = _fortmater.GlobalRNameGetter(szBody, status, message.szQuota);
                        break;
                    case EnumsAtLarge.EnumContainer.TransactionQuota.MerchantPayment:
                    case EnumsAtLarge.EnumContainer.TransactionQuota.CustomerTransfer:
                    case EnumsAtLarge.EnumContainer.TransactionQuota.AccountDeposit:
                        break;
                    case EnumsAtLarge.EnumContainer.TransactionQuota.AirtimePurchase:
                            message.szRName = message.szQuota.ToString();
                        break;
                    case EnumsAtLarge.EnumContainer.TransactionQuota.LoanDebit:
                        message.szRName = message.szQuota.ToString();
                        message.dBalance = _fortmater.CashConverter(moneyArray[2]);

                        if (szBody.Contains("charged"))
                        {
                            message.szQuota = EnumsAtLarge.EnumContainer.TransactionQuota.LoanCredit;
                            message.szRName = message.szQuota.ToString();
                        }
                        
                        break;
                }
                break;
            }
            return message;
        }
    }
}
