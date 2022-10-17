using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtensibleMarkupAtLarge
{
    class X_XMLFormartToString
    {
        public void BeginKCBFormatToString(X_XMLProperties message)
        {
            _ExtractBodyForKCB(message);
        }
        private X_XMLProperties _ExtractBodyForKCB(X_XMLProperties message)
        {


            return message;
        }
        public void BeginMPESAFormatToString(X_XMLProperties message)
        {
             _ExtractBodyForMPESA(message);
        }

        private X_XMLProperties _ExtractBodyForMPESA(X_XMLProperties message)
        {
                                                                                            string szBody = message.szBody;
                                                                                            string DicedDate = string.Empty;
                                                                                            string DicedPrice = string.Empty;
                                                                                            string DicedBalance = string.Empty;
                                                                                            string DicedBody = string.Empty;
                                                                                            string DicedTransactionCost = string.Empty;
                                                                                            string szID = string.Empty;
                                                                                            bool bCheck = false;
                                                                                            bool bECheck = false;
                                                                                            bool bEeCheck = false;
                                                                                            bool bWrongData = false;
                                                                                            bool bFuliza = false;

            string [] status = new string[3];

            string[] wordsArray = szBody.Split(' ');
            string[] sentencesArray = szBody.Split('.');

            while (true)
            {
                if (wordsArray[0] == "Failed."  ||
                    wordsArray[2] == "unable"   ||
                    wordsArray[0] == "PGH2GTE60C\n[20210717;" ||
                    wordsArray[1] == "failed,"  ||
                    wordsArray[0] == "You"      ||
                    wordsArray[1] == "MSISDN"   ||
                    wordsArray[0] == "An"       ||
                    wordsArray[4] == "balance"  && 
                    wordsArray[5] == "was:")
                {
                    bWrongData = true;
                    break;
                }

                if (wordsArray[6] == "Paybill")
                {
                    szID = "Paybill";

                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "account";

                    break;
                }
                if (wordsArray[5] == "airtime")
                {
                    szID = "airtime";

                    status[0] = "bought";
                    status[1] = "of";
                    status[2] = "airtime";

                    break;
                }
                if (wordsArray[5] == "paid" || wordsArray[3] == "paid")
                {
                    szID = "paid";

                    status[0] = "Confirmed";
                    status[1] = "paid";
                    status[2] = "to";

                    break;
                }
                if ( wordsArray[3] == "sent" || wordsArray[5] == "sent" || wordsArray[5] == "EASYFLOAT")
                {
                    szID = "sent";
                    if (wordsArray[5] == "EASYFLOAT")
                    {
                        szID = "airtime";
                    }

                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "to";

                    break;
                }
                if (wordsArray[4] == "received" || wordsArray[3] == "received")
                {
                    szID = "received";

                    status[0] = "received";
                    status[1] = "from";
                    status[2] = "from";

                    break;
                }
                if (wordsArray[5] == "PMWithdraw" || wordsArray[5] == "AMWithdraw")
                {
                    szID = "Withdraw";

                    status[0] = "PMWithdraw";
                    if (wordsArray[5] == "AMWithdraw")
                    {
                        status[0] = "AMWithdraw";
                    }
                    status[1] = "from";
                    status[2] = "from";

                    break;
                }
                if (wordsArray[15] == "Fuliza" || wordsArray[2] == "Fuliza")
                {
                    szID = "borrowed";

                    status[0] = "Confirmed";
                    status[1] = "from";
                    status[2] = "outstanding";

                    bWrongData = true;
                    bFuliza = true;

                    break;
                }

                //message.Code = wordsArray[0];
                //message.TransactionStatus = wordsArray[3];
                //message.CashAmount = wordsArray[4];
                //message.RName = wordsArray[6] + " " + wordsArray[8];
                //message.RPhoneNo = wordsArray[9];
                //message.Balance = wordsArray[20];
                //message.RDate = wordsArray[11];

                //Debug.WriteLine(message.Code
                //            + " = " + message.TransactionStatus
                //            + " = " + message.CashAmount
                //            + " = " + message.RName
                //            + " = " + message.RPhoneNo
                //            + " = " + message.Balance
                //            + " = " + message.RDate);

                break;
            }
            if (!bWrongData)
            {
                xx_RegexFilter(szBody,
                            message,
                            wordsArray,
                            status,
                            szID);

                Debug.WriteLine(message.Code
                                + " = " + message.TransactionStatus
                                + " = " + message.RAccNo
                                + " = " + message.RName
                                + " = " + message.RDate
                                + " = " + message.CashAmount
                                + " = " + message.Balance
                                + " = " + message.TransactionCost);
            }
            if (bFuliza)
            {
                xx_FulizaLoanFormater(szBody,
                                    message,
                                    wordsArray,
                                    status,
                                    szID);

                Debug.WriteLine(message.Code
                                + " = " + message.RName
                                + " = " + message.TransactionStatus
                                + " = " + message.FulizaAmount
                                + " = " + message.Charges
                                + " = " + message.FulizaBorrowed);
            }
            return message;
            
        }

        private string xx_RegexFilter(string szvBody, 
                                    X_XMLProperties vmessage, 
                                    string[] wordsArray, 
                                    string [] vStatus,
                                    string szvID)
        {
                                                                                                string szCashAmountAfter = string.Empty;
                                                                                                string szNameAfter = string.Empty;
                                                                                                string szCashBalanceAfter = string.Empty;
                                                                                                string szDateAfter = string.Empty;
                                                                                                string szPhoneNoAfter = string.Empty;
                                                                                                string[]? SzACashAmount = null;
                                                                                                string[]? SzACashBalance = null;
                                                                                                string[]? SzADate = null;
                                                                                                string[]? SzARName = null;
                                                                                                string[]? SzAPhoneNo = null;

            List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

            try
            {
                //var regCash = new Regex(@"received(\b(.*)(\s+from))");
                var regCash = new Regex(string.Format(@"{0}(\b(.*)(\s+{1}))", vStatus[0], vStatus[1]));
                var regBalance = new Regex(@"balance is(\s\b(.*)\D)");
                var regCostings = new Regex(@"Transaction cost,(\s\b(.*)\D)");
                var regDate = new Regex(@"on(\b(.*)(\s+at))");
                //var regRName = new Regex(string.Format(@"{0}(\b(.*)(\s)\b(on|in)(\s.*\w\s))", vStatus[2]));
                var regPayPall = new Regex(@"via(\b(.*)(\s)\b(on|in)(\s.*\w\s))");
                var regRPhoneNo254 = new Regex(@"254((.*)(\s+)on(.*)(\s+)at)");
                var regRPhoneNo07 = new Regex(@"\s07((.*)(\s+on))");
                var regRAccountNo = new Regex(@"account((.*)(\s+)on)");
                var regWithdraw = new Regex(@"from ((.*)(\s+)New)");

                //var rNameBefore = regRName.Matches(szvBody);
                var rPayPallBefore = regPayPall.Matches(szvBody);
                var rRPhoneNo254 = regRPhoneNo254.Matches(szvBody);
                var rRPhoneNo07 = regRPhoneNo07.Matches(szvBody);
                var rRAccountNo = regRAccountNo.Matches(szvBody);
                var rWithdraw = regWithdraw.Matches(szvBody);

                bool bPayPall = rPayPallBefore.Count.Equals(0);
                //bool bNameBefore = rNameBefore.Count.Equals(0);
                bool bRPhoneNo254 = rRPhoneNo254.Count.Equals(0);
                bool bRPhoneNo07 = rRPhoneNo07.Count.Equals(0);
                bool bRAccountNo = rRAccountNo.Count.Equals(0);
                bool bWithdraw = rWithdraw.Count.Equals(0);


                try
                {
                    while (true)
                    {
                        if (bRPhoneNo07 == false)
                        {
                            szPhoneNoAfter = rRPhoneNo07[0].Value;
                        }
                        //if (bNameBefore == false)
                        //{
                        //    szNameAfter = rNameBefore[0].Value;
                        //    //szPhoneNoAfter = rNameBefore[0].Value;
                        //}
                        if (bRPhoneNo254 == false)
                        {
                            szPhoneNoAfter = rRPhoneNo254[0].Value;
                        }
                        if (bRAccountNo == false)
                        {
                            szPhoneNoAfter = rRAccountNo[0].Value;
                        }
                        if (szvID != "received")
                        {
                            vmessage.Charges = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regCostings)[2]));
                        }
                        if (bPayPall == false)
                        {
                            szPhoneNoAfter = rPayPallBefore[0].Value;
                            //szNameAfter = rNameBefore[0].Value;
                        }
                        if(bWithdraw == false)
                        {
                            szNameAfter = rWithdraw[0].Value;
                        }
                        break;
                    }

                    //SzARName = szNameAfter.Split(' ');
                    SzAPhoneNo = szPhoneNoAfter.Split(' ');
                }
                catch (Exception)
                {

                }

                vmessage.RName = xx_GetAndSetContactName(szvBody, szvID, vStatus);

                //Regex r = new Regex("\\s+");
                //SzARName[1] = r.Replace(SzARName[1], "");

                //vmessage.RName = BodyToValueArray(szvBody, regRName)[1] + " " + BodyToValueArray(szvBody, regRName)[2] + " " + BodyToValueArray(szvBody, regRName)[3];
                vmessage.Balance = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regBalance)[2]));
                vmessage.Code = wordsArray[0];
                vmessage.TransactionStatus = szvID;
                vmessage.RDate = BodyToValueArray(szvBody, regDate)[1];
                vmessage.CashAmount = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regCash)[1]));
                if (szvID != "airtime")
                {
                    vmessage.RPhoneNo = SzAPhoneNo[1];
                }

                if(vmessage.RPhoneNo == string.Empty )
                {
                    string[] SzAName = vmessage.RName.Split(' ');

                    string name = "";

                    foreach (string item in SzAName)
                    {
                       name = name + item;
                    }
                    vmessage.RPhoneNo = name;
                }


                switch (szvID)
                {
                    case "paid":
                        //vmessage.RPhoneNo = SzARName[1] + SzARName[3];
                        break;
                    case "Paybill":
                        //vmessage.RAccNo = SzARName[1];
                        //vmessage.RPhoneNo = SzARName[1];
                        break;
                    case "airtime":
                        //if (SzARName[1] == "on")
                        //{
                        //    vmessage.RName = "airtime";
                        //}
                        //if (SzARName[1] == "EASYFLOAT")
                        //{
                        //    vmessage.Quota = SzARName[1];
                        //}
                        break;
                    case "sent":
                        //if (SzARName[1] == "SIDIAN")
                        //{
                        //    vmessage.Quota = SzARName[1];
                        //}
                        //if(SzARName[1] == "GO")
                        //{
                        //    vmessage.RPhoneNo = SzAPhoneNo[1];
                        //}
                        break;
                    case "received":
                        //if (SzARName[1] == "SIDIAN")
                        //{
                        //    vmessage.Quota = SzARName[1];
                        //}
                        //if (bPayPall == false)
                        //{
                        //    vmessage.RAccNo = SzARName[1]+ SzARName[2];
                        //    vmessage.Quota = SzAPhoneNo[1];
                        //    vmessage.RPhoneNo = SzARName[1]+ SzAPhoneNo[0]+SzAPhoneNo[1];
                        //}
                        break;
                    case "Withdraw":
                        //vmessage.RPhoneNo =
                        //vmessage.RAccNo = 
                        //vmessage.RName = SzARName[1] + " " + SzARName[2] + " " + SzARName[3] + " " + SzARName[4] + " " + SzARName[5] + " " + SzARName[6];
                        break;
                }
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw new Exception(erro);
            }

            xml_prop.Add(vmessage);

            return vStatus[0];
        }

        private string xx_GetAndSetContactName(string szvBody, 
                                               string szvID,
                                               string[] vSzAStatus)
        {
                                                                                                        string szNameAfter = string.Empty;
                                                                                                        string szContactName = string.Empty;

            while (true)
            {
                var regRName = new Regex(string.Format(@"{0}(\b(.*)(\s)\b(on|in)(\s.*\w\s))", vSzAStatus[2]));

                var rNameBefore = regRName.Matches(szvBody);

                bool bNameBefore = rNameBefore.Count.Equals(0);

                if (bNameBefore == true)
                {
                    break;
                }

                //szNameAfter = rNameBefore[0].Value;
                ////szPhoneNoAfter = rNameBefore[0].Value;

                //string[] SzARName = szNameAfter.Split(' ');

                switch (szvID)
                {
                    case "paid":
                        break;
                    case "Paybill":
                        szContactName = BodyToValueArray(szvBody, regRName)[0] + " " + BodyToValueArray(szvBody, regRName)[1];
                        break;
                    case "airtime":
                        szContactName = BodyToValueArray(szvBody, regRName)[1] + " " + BodyToValueArray(szvBody, regRName)[2] + " " + BodyToValueArray(szvBody, regRName)[3];
                        break;
                    case "sent":
                        break;
                    case "received":
                        break;
                    case "Withdraw":
                        break;
                }
                break;
            }
            return szContactName;
        }
        private string xx_FulizaLoanFormater(string szvBody,
                                    X_XMLProperties vmessage,
                                    string[] wordsArray,
                                    string[] vStatus,
                                    string szvID)
        {
                                                                                        string cashFulizaAmountAfter = string.Empty;
                                                                                        string[]? SzAFulizaAmount = null;
                                                                                        string[]? SzAFulizaCharge = null;
                                                                                        string[]? SzAFulizaDebt = null;
                                                                                        string[]? SzAFulizaLimit = null;
                                                                                        bool extraCheck = false;

            List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

            try
            {
                var regt = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");
                var caBefore = regt.Matches(szvBody);
                extraCheck = caBefore[0].Success;
            }
            catch
            {
                extraCheck = false;
            }
            try
            {
                if (!extraCheck)
                {
                    var regFulizaAmount = new Regex(@"Ksh(\b(.*)(\s)\b(Fee))");
                    var regFulizaCharge = new Regex(@"charged (\b(.*)(\s)\b(Total))");
                    var regFulizaDebt = new Regex(@"outstanding amount is (\b(.*)(\s)\b(due))");

                    vmessage.FulizaAmount = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regFulizaAmount)[1]));
                    vmessage.Charges = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regFulizaCharge)[2]));
                    vmessage.FulizaBorrowed = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regFulizaDebt)[4]));
                }
                if (extraCheck)
                {
                    var regFulizaLimit = new Regex(@"M-PESA limit is (\b(.*)(\s)\b(M-PESA))");
                    var regFulizaAmount = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");

                    vmessage.FulizaLimit = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody,regFulizaLimit)[4]));
                    vmessage.FulizaAmount = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regFulizaAmount)[2]));
                }
                vmessage.Code = wordsArray[0];
                vmessage.TransactionStatus = szvID;
                vmessage.RName = vmessage.Quota = "Fuliza";

                xml_prop.Add(vmessage);
            }
            catch (Exception ex)
            {
                var erro = ex.Message;
                throw new Exception(erro);
            }
            

            return vStatus[0];
        }

        public string[] BodyToValueArray(string szvBody, Regex RBody)
        {
                                                                                        string szAmountAfter = string.Empty;

            var AmountBefore = RBody.Matches(szvBody);
            try
            {
                szAmountAfter = AmountBefore[0].Value;
            }
            catch
            {
                szAmountAfter = "Null Null";
            }
            string[] SzAmount = szAmountAfter.Split(' ');

            return SzAmount;
        }

        public string CashConverter(string vValue)
        {

                                                                                        string strFinValue = string.Empty;
                                                                                        string strFiValue = string.Empty;
            if (vValue.StartsWith("Ksh"))
            {
                string strNoKsh = vValue.Remove(0, 3);
                string[] staNoSecondDot = strNoKsh.Split(".");
                string strComValue = staNoSecondDot[0] + "." + staNoSecondDot[1];
                strFinValue = strComValue.Replace(",", "");
            }
            else
            {
                string[] staNoSecondDot = vValue.Split(".");
                string strComValue = staNoSecondDot[0] + "." + staNoSecondDot[1];
                strFinValue = strComValue.Replace(",", "");
            }
            strFiValue = strFinValue;

            return strFiValue;
        }
    }
}
