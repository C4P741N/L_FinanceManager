using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtensibleMarkupAtLarge
{
    class X_XMLFormartToString
    {
        private Z_Formaters.Formaters xFormat = null;
        public void BeginKCBFormatToString(X_XMLProperties message)
        {
            xFormat = new Z_Formaters.Formaters();

            _ExtractBodyForKCB(message);
        }
        private X_XMLProperties _ExtractBodyForKCB(X_XMLProperties message)
        {
            string szBody = message.szBody;
            List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

            while (true)
            {
                string[] wordsArray = szBody.Split(' ');
                string[]? status = new string[2];

                if (szBody == "Ksh 14000.00 sent to KCB Pay Bill 522522 for account 1206232161 has been received on 05/06/2022 at 05:09 PM. M-PESA ref QF5786HI4J.Dial *522# to pay your bills.")
                {

                }



                if (szBody.Contains("sent to"))
                {
                    status[0] = "";
                    status[1] = "sent";

                    message.CashAmount = xFormat.GetCashAmount(szBody, status);
                    message.RAccNo = xFormat.GetAccountNumber(szBody);
                    message.RDate = xFormat.GetDate(szBody);
                    message.Code = xFormat.GetMpesaCode(szBody);
                    message.RName = xFormat.RNameCreatorFromAccNo(message.RAccNo);

                    if (szBody.Contains("Pay Bill"))
                    {
                        message.Quota = "Pay Bill Payment";
                        message.PayBill_TillNo = xFormat.GetPayBillNo(szBody);
                    }

                    break;
                }
                if (szBody.Contains("has transfered"))
                {
                    status[0] = "KES";
                    status[1] = "to";

                    message.Quota = "Deposit";
                    message.Code = xFormat.GetMpesaCode(szBody);

                    message.CashAmount = xFormat.GetCashAmount(szBody, status);
                    message.RName = xFormat.GetKCBContactName(szBody);
                }

                xml_prop.Add(message);

                break;
            }

            return message;
        }
        public void BeginMPESAFormatToString(X_XMLProperties message)
        {
            xFormat = new Z_Formaters.Formaters();

             _ExtractBodyForMPESA(message);
        }

        private X_XMLProperties _ExtractBodyForMPESA(X_XMLProperties message)
        {
                                                                                            string szBody = message.szBody;
                                                                                            string szID = string.Empty;
                                                                                            bool bWrongData = false;
                                                                                            bool bFuliza = false;
                                                                                            string [] status = new string[3];

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
                if (szBody.Contains("PMWithdraw") || szBody.Contains("AMWithdraw") )
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

        private X_XMLProperties xx_RegexFilter(string szvBody, 
                                    X_XMLProperties vmessage, 
                                    string[] wordsArray, 
                                    string [] vStatus,
                                    string szvID)
        {
                                                                                                bool bIsArrayNull;
                                                                                                List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

            try
            {
                while (true)
                {

                    bIsArrayNull = vStatus.Contains(null);

                    if (bIsArrayNull)
                        break;

                    vmessage.RName = xFormat.GetContactName(szvBody, szvID, vStatus);

                    vmessage.Balance = xFormat.GetCashBalance(szvBody);

                    vmessage.CashAmount = xFormat.GetCashAmount(szvBody, vStatus);

                    vmessage.RAccNo = xFormat.GetAccountNumber(szvBody);

                    vmessage.Charges = xFormat.GetCharges(szvBody);

                    vmessage.RDate = xFormat.GetDate(szvBody);

                    vmessage.RPhoneNo = xFormat.GetPhoneNumber(szvBody, szvID, vStatus).ToLower();

                    vmessage.Code = wordsArray[0];

                    if (vmessage.RName.Contains("Airtime"))
                        vmessage.RPhoneNo = xFormat.StringSplitAndJoin(vmessage.RName).ToLower();
                    
                    if (szvBody.Contains("received") || szvBody.Contains("Give"))
                        vmessage.Quota = "Deposit";

                    if (szvBody.Contains("airtime") ||
                        szvBody.Contains("Safaricom Limited") == true ||
                        szvBody.Contains("Safaricom Offers") == true)
                    {
                        vmessage.Quota = "Airtime Purchase";
                        break;
                    }

                    if (szvBody.Contains("sent to"))
                        vmessage.Quota = "Customer Transfer";
                    
                    if (szvBody.Contains("withdraw") || szvID == "withdraw")
                        vmessage.Quota = "Withdrawal";

                    if (szvBody.Contains("for account"))
                    {
                        if (szvBody.Contains("Safaricom Limited") == false || szvBody.Contains("Safaricom Offers") == false)
                        {
                            vmessage.Quota = "Pay Bill Payment";
                            //vmessage.PayBill_TillNo = szvID;
                        }
                    }

                    if (szvBody.Contains("paid to"))
                    {
                        vmessage.Quota = "Merchant Payment";
                        //vmessage.PayBill_TillNo = szvID;
                    }

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

        private X_XMLProperties xx_FulizaLoanFormater(string szvBody,
                                    X_XMLProperties vmessage,
                                    string[] wordsArray,
                                    string szvID)
        {
                                                                                                                bool bIsBeforeBeforeEmpty;
                                                                                                                List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

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

                        vmessage.FulizaAmount = Convert.ToDouble(xFormat.CashConverter(xFormat.BodyToValueArray(szvBody, regFulizaAmount)[1]));
                        vmessage.Charges = Convert.ToDouble(xFormat.CashConverter(xFormat.BodyToValueArray(szvBody, regFulizaCharge)[2]));
                        vmessage.FulizaBorrowed = Convert.ToDouble(xFormat.CashConverter(xFormat.BodyToValueArray(szvBody, regFulizaDebt)[4]));

                        vmessage.Quota = "Fuliza Borrowed";
                    }
                    if (bIsBeforeBeforeEmpty == false)
                    {
                        var regFulizaLimit = new Regex(@"M-PESA limit is (\b(.*)(\s)\b(M-PESA))");
                        var regFulizaAmount = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");

                        vmessage.FulizaLimit = Convert.ToDouble(xFormat.CashConverter(xFormat.BodyToValueArray(szvBody, regFulizaLimit)[4]));
                        vmessage.FulizaAmount = Convert.ToDouble(xFormat.CashConverter(xFormat.BodyToValueArray(szvBody, regFulizaAmount)[2]));

                        vmessage.Quota = "Fuliza Paid";
                    }
                    vmessage.Code = wordsArray[0];
                    vmessage.RName = "Fuliza";

                    vmessage.RPhoneNo = vmessage.RName.ToLower();

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
