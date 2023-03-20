using MSota.BaseFormaters;
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

            string[] wordsArray = szBody.Split(' ');

            while (true)
            {
                if (szBody.Contains("Failed") ||
                    szBody.Contains("balance was") ||
                    szBody.Contains("currently underway") ||
                    szBody.Contains("cancelled the transaction") ||
                    szBody.Contains("failed"))
                {
                    bWrongData = true;
                    break;
                }

                if (szBody.Contains("Paybill"))
                {
                    szID = "paybill";

                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "account";

                    break;
                }
                if (szBody.Contains("airtime"))
                {
                    szID = "airtime";

                    status[0] = "bought";
                    status[1] = "of";
                    status[2] = "airtime";

                    break;
                }
                if (szBody.Contains("paid to"))
                {
                    szID = "paid";

                    status[0] = "Confirmed";
                    status[1] = "paid";
                    status[2] = "to";

                    break;
                }
                if (szBody.Contains("sent to") || szBody.Contains("EASYFLOAT"))
                {
                    szID = "sent";

                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "to";

                    break;
                }
                if (szBody.Contains("received"))
                {
                    szID = "received";

                    status[0] = "received";
                    status[1] = "from";
                    status[2] = "from";

                    break;
                }
                if (szBody.Contains("PMWithdraw") || szBody.Contains("AMWithdraw"))
                {
                    szID = "withdraw";

                    status[0] = "PMWithdraw";
                    if (szBody.Contains("AMWithdraw"))
                    {
                        status[0] = "AMWithdraw";
                    }
                    status[1] = "from";
                    status[2] = "from";

                    break;
                }
                if (szBody.Contains("Fuliza"))
                {
                    szID = "borrowed";

                    status[0] = "Confirmed";
                    status[1] = "from";
                    status[2] = "outstanding";

                    bWrongData = true;
                    bFuliza = true;

                    break;
                }

                break;
            }
            if (bWrongData == false)
            {
                xx_RegexFilter(szBody,
                            message,
                            wordsArray,
                            status,
                            szID);
            }
            if (bFuliza == true)
            {
                xx_FulizaLoanFormater(szBody,
                                    message,
                                    wordsArray,
                                    szID);
            }
            return message;

        }

        private IXmlProps xx_RegexFilter(string szvBody,
                                    IXmlProps vmessage,
                                    string[] wordsArray,
                                    string[] vStatus,
                                    string szvID)
        {
            bool bIsArrayNull;
            List<IXmlProps> xml_prop = new List<IXmlProps>();

            try
            {
                while (true)
                {

                    bIsArrayNull = vStatus.Contains(null);

                    if (bIsArrayNull)
                        break;

                    vmessage.szRName = _fortmater.GetContactName(szvBody, szvID, vStatus);

                    vmessage.dBalance = _fortmater.GetCashBalance(szvBody);

                    vmessage.dCashAmount = _fortmater.GetCashAmount(szvBody, vStatus);

                    vmessage.szRAccNo = _fortmater.GetAccountNumber(szvBody);

                    vmessage.dCharges = _fortmater.GetCharges(szvBody);

                    //vmessage.szRDate = _fortmater.GetDate(szvBody);

                    vmessage.szRPhoneNo = _fortmater.GetPhoneNumber(szvBody, szvID, vStatus).ToLower();

                    vmessage.szCode = wordsArray[0];

                    if (vmessage.szRName.Contains("Airtime"))
                        vmessage.szRPhoneNo = _fortmater.StringSplitAndJoin(vmessage.szRName).ToLower();

                    if (szvBody.Contains("received") || szvBody.Contains("Give"))
                        vmessage.szQuota = "Deposit";

                    if (szvBody.Contains("airtime") ||
                        szvBody.Contains("Safaricom Limited") == true ||
                        szvBody.Contains("Safaricom Offers") == true)
                    {
                        vmessage.szQuota = "Airtime Purchase";
                        break;
                    }

                    if (szvBody.Contains("sent to"))
                        vmessage.szQuota = "Customer Transfer";

                    if (szvBody.Contains("withdraw") || szvID == "withdraw")
                        vmessage.szQuota = "Withdrawal";

                    if (szvBody.Contains("for account"))
                    {
                        if (szvBody.Contains("Safaricom Limited") == false || szvBody.Contains("Safaricom Offers") == false)
                        {
                            vmessage.szQuota = "Pay Bill Payment";
                            //vmessage.PayBill_TillNo = szvID;
                        }
                    }

                    if (szvBody.Contains("paid to"))
                        vmessage.szQuota = "Merchant Payment";

                    break;
                }
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw new Exception(erro);
            }

            xml_prop.Add(vmessage);

            return vmessage;
        }

        private IXmlProps xx_FulizaLoanFormater(string szvBody,
                                                IXmlProps vmessage,
                                                string[] wordsArray,
                                                string szvID)
        {
            bool bIsBeforeBeforeEmpty;
            List<IXmlProps> xml_prop = new List<IXmlProps>();

            try
            {
                while (true)
                {
                    var regt = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");
                    var rBefore = regt.Matches(szvBody);

                    bIsBeforeBeforeEmpty = rBefore.Count.Equals(0);

                    if (bIsBeforeBeforeEmpty == true)
                    {
                        var regFulizaAmount = new Regex(@"Ksh(\b(.*)(\s)\b(Fee))");
                        var regFulizaCharge = new Regex(@"charged (\b(.*)(\s)\b(Total))");
                        var regFulizaDebt = new Regex(@"outstanding amount is (\b(.*)(\s)\b(due))");

                        vmessage.dFulizaAmount = Convert.ToDouble(_fortmater.CashConverter(_fortmater.BodyToValueArray(szvBody, regFulizaAmount)[1]));
                        vmessage.dCharges = Convert.ToDouble(_fortmater.CashConverter(_fortmater.BodyToValueArray(szvBody, regFulizaCharge)[2]));
                        vmessage.dFulizaBorrowed = Convert.ToDouble(_fortmater.CashConverter(_fortmater.BodyToValueArray(szvBody, regFulizaDebt)[4]));

                        vmessage.szQuota = "Fuliza Borrowed";
                    }
                    if (bIsBeforeBeforeEmpty == false)
                    {
                        var regFulizaLimit = new Regex(@"M-PESA limit is (\b(.*)(\s)\b(M-PESA))");
                        var regFulizaAmount = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");

                        vmessage.dFulizaLimit = Convert.ToDouble(_fortmater.CashConverter(_fortmater.BodyToValueArray(szvBody, regFulizaLimit)[4]));
                        vmessage.dFulizaAmount = Convert.ToDouble(_fortmater.CashConverter(_fortmater.BodyToValueArray(szvBody, regFulizaAmount)[2]));

                        vmessage.szQuota = "Fuliza Paid";
                    }
                    vmessage.szCode = wordsArray[0];
                    vmessage.szRName = "Fuliza";

                    vmessage.szRPhoneNo = vmessage.szRName.ToLower();

                    break;
                }

                xml_prop.Add(vmessage);
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw new Exception(erro);
            }

            return vmessage;
        }
    }
}
