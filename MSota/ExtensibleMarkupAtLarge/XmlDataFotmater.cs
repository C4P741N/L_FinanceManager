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
            List<IXmlProps> xml_prop = new List<IXmlProps>();

            while (true)
            {
                string[] wordsArray = szBody.Split(' ');
                string[]? status = new string[2];

                if (szBody.Contains("sent to"))
                {
                    status[0] = "";
                    status[1] = "sent";

                    message.dCashAmount = _fortmater.GetCashAmount(szBody, status);
                    message.szRAccNo = _fortmater.GetAccountNumber(szBody);
                    //message.szRDate = _fortmater.GetDate(szBody);
                    message.szCode = _fortmater.GetMpesaCode(szBody);
                    message.szRName = _fortmater.RNameCreatorFromAccNo(message.szRAccNo);

                    if (szBody.Contains("Pay Bill"))
                    {
                        message.szQuota = "Pay Bill Payment";
                        message.szPayBill_TillNo = _fortmater.GetPayBillNo(szBody);
                    }

                    break;
                }
                if (szBody.Contains("has transfered"))
                {
                    status[0] = "KES";
                    status[1] = "to";

                    message.szQuota = "Deposit";
                    message.szCode = _fortmater.GetMpesaCode(szBody);

                    message.dCashAmount = _fortmater.GetCashAmount(szBody, status);
                    message.szRName = _fortmater.GetKCBContactName(szBody);
                }

                xml_prop.Add(message);

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
                    message.szQuota = "Account Deposit";

                    szID = "received";

                    status[0] = "";
                    status[1] = "from";
                }
                if (szBody.Contains("airtime") ||
                    szBody.Contains("Safaricom Limited") == true ||
                    szBody.Contains("Safaricom Offers") == true)
                {
                    message.szQuota = "Airtime Purchase";

                    szID = "airtime";

                    if(szBody.Contains("airtime for"))
                    {
                        status[0] = "";
                        status[1] = "for";
                    }
                }
                if (szBody.Contains("paid to"))
                {
                    message.szQuota = "Merchant Payment";
                    szID = "paid";

                    status[0] = "paid";
                    status[1] = "to";
                }
                if (szBody.Contains("sent to") || 
                    szBody.Contains("EASYFLOAT") || 
                    szBody.Contains("Paybill"))
                {
                    message.szQuota = "Customer Transfer";
                    szID = "sent";

                    status[0] = "sent";
                    status[1] = "to";
                }
                if (szBody.Contains("PMWithdraw") || 
                    szBody.Contains("AMWithdraw"))
                {
                    message.szQuota = "Account Withdrawal";
                    szID = "withdraw";

                    status[0] = "";
                    status[1] = "from";
                    status[2] = szID;
                }
                if (szBody.Contains("Fuliza"))
                {
                    message.szQuota = "Account Loan";
                    szID = "borrowed";

                    status[0] = "Confirmed";
                    status[1] = "from";
                    status[2] = "outstanding";
                }

                if (string.IsNullOrEmpty(message.szQuota))
                    break;

                message.szCode = wordsArray[0];
                message.szRName = _fortmater.GlobalRNameGetter(szBody, status);
                message.szRAccNo = _fortmater.GlobalAccNoAndPhoneNoGetter(szBody);
                moneyArray = _fortmater.GlobalCashGetter(szBody);

                if (string.IsNullOrEmpty(message.szRName))
                    message.szRName = "Null";

                message.dCashAmount = _fortmater.CashConverter(moneyArray[0]);
                message.dBalance = _fortmater.CashConverter(moneyArray[1]);
                if(moneyArray.Count() == 3)
                    message.dCharges = _fortmater.CashConverter(moneyArray[2]);

                switch (message.szQuota)
                {
                    case "Account Withdrawal":
                        message.szRName = _fortmater.GlobalRNameGetter(szBody, status);
                        break;
                    case "Merchant Payment":
                    case "Customer Transfer":
                    case "Account Deposit":
                        break;
                    case "Airtime Purchase":
                            message.szRName = message.szQuota;
                        break;
                    case "Account Loan":
                        message.szRName = message.szQuota;
                        message.dBalance = _fortmater.CashConverter(moneyArray[2]);

                        if (szBody.Contains("charged"))
                        {
                            message.szQuota = "Loan Payment";
                            message.szRName = message.szQuota;
                        }
                        
                        break;
                }
                break;
            }
            return message;
        }
    }
}
