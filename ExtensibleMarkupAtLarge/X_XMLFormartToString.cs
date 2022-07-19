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
                    //}

                    szID = "Paybill";

                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "account";

                    break;
                }
                //if (wordsArray[5] == "EASYFLOAT")
                //{
                //    szID = "airtime";

                //    status[0] = "Confirmed";//{ "Confirmed", "sent", "to"};
                //    status[1] = "sent";
                //    status[2] = "to";

                //    break;
                //}
                if (wordsArray[5] == "airtime")
                {
                    szID = "airtime";

                    //string[] status = { "bought", "of", "airtime" };
                    status[0] = "bought";
                    status[1] = "of";
                    status[2] = "airtime";

                    break;
                }
                if (wordsArray[5] == "paid" || wordsArray[3] == "paid")
                {
                    szID = "paid";

                    //string[] status = { "Confirmed", "paid", "to" };
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

                    //string[] status = { "Confirmed", "sent", "to" };
                    status[0] = "Confirmed";
                    status[1] = "sent";
                    status[2] = "to";

                    break;
                }
                /////////TEST
                if (wordsArray[4] == "received" || wordsArray[3] == "received")
                {
                    szID = "received";
                    //string[] status = { "received", "from", "from" };
                    status[0] = "received";
                    status[1] = "from";
                    status[2] = "from";

                    break;
                }
                if (wordsArray[5] == "PMWithdraw" || wordsArray[5] == "AMWithdraw")
                {
                    DicedBalance = sentencesArray[2];
                    DicedTransactionCost = sentencesArray[4];

                    string[] dicedWordsPrice = DicedBalance.Split(' ');
                    string[] dicedWordTransactPrice = DicedTransactionCost.Split(' ');

                    bCheck = dicedWordsPrice.Count().Equals(15);
                    bECheck = dicedWordsPrice.Count().Equals(14);
                    bEeCheck = dicedWordsPrice.Count().Equals(16);
                    bool bEeeCheck = dicedWordsPrice.Count().Equals(18);

                    message.Code = wordsArray[0];
                    message.TransactionStatus = wordsArray[5];
                    message.CashAmount = wordsArray[6];
                    message.RAccNo = wordsArray[8];
                    message.RName = wordsArray[10] + " " + wordsArray[11] + " " + wordsArray[12];
                    message.TransactionCost = dicedWordTransactPrice[3];
                    if (!bCheck && dicedWordsPrice[11] != "balance" && !bEeCheck)
                    {
                        message.Balance = dicedWordsPrice[16];
                    }
                    if(!bCheck && !bECheck && !bEeCheck && dicedWordsPrice[16] == "balance")
                    {
                        message.Balance = dicedWordsPrice[18];
                    }
                    if(bCheck)
                    {
                        message.Balance = dicedWordsPrice[14];
                    }
                    if (!bCheck && dicedWordsPrice[11] == "balance" && !bEeCheck)
                    {
                        message.Balance = dicedWordsPrice[13];
                    }
                    if (bEeCheck)
                    {
                        message.Balance = dicedWordsPrice[15];
                    }
                    if (bEeeCheck)
                    {
                        message.Balance = dicedWordsPrice[17];
                    }


                    Debug.WriteLine(message.Code
                            + " = " + message.CashAmount
                            + " = " + message.TransactionStatus
                            + " = " + message.RAccNo
                            + " = " + message.RName
                            + " = " + message.Balance
                            + " = " + message.TransactionCost);

                    break;
                }
                if (wordsArray[15] == "Fuliza" || wordsArray[2] == "Fuliza")
                {
                    szID = "borrowed";
                    //string[] status = { "received", "from", "from" };
                    status[0] = "Confirmed";
                    status[1] = "from";
                    status[2] = "outstanding";

                    bWrongData = true;
                    bFuliza = true;

                    break;
                }
                //if (wordsArray[2] == "Fuliza")
                //{
                //    szID = "borrowed";
                //    //string[] status = { "received", "from", "from" };
                //    status[0] = "Confirmed";
                //    status[1] = "from";
                //    status[2] = "outstanding";

                //    break;
                //}



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
                RegexFilter(szBody,
                            message,
                            wordsArray,
                            status,
                            szID);

                Debug.WriteLine(message.Code
                            + " = " + message.RAccNo
                            + " = " + message.RName
                            + " = " + message.RDate
                            + " = " + message.TransactionStatus
                            + " = " + message.CashAmount
                            + " = " + message.Balance
                            + " = " + message.TransactionCost);
            }
            if (bFuliza)
            {
                FulizaLoanFormater(szBody,
                                    message,
                                    wordsArray,
                                    status,
                                    szID);

                Debug.WriteLine(message.Code
                            + " = " + message.RAccNo
                            + " = " + message.RName
                            + " = " + message.RDate
                            + " = " + message.TransactionStatus
                            + " = " + message.CashAmount
                            + " = " + message.Balance
                            + " = " + message.TransactionCost);
            }
            return message;
        }

        public string RegexFilter(string szvBody, 
                                    X_XMLProperties vmessage, 
                                    string[] wordsArray, 
                                    string [] vStatus,
                                    string szvID)
        {
            string? cashAmountAfter = null;
            string? rNameAfter = null;
            string? cashBalanceAfter = null;
            string? dateAfter = null;
            string[]? SzACashAmount = null;
            string[]? SzACashBalance = null;
            string[]? SzADate = null;
            string[]? SzARName = null;

            List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();

            ////////////////////////////--Received
            //var regCash = new Regex(@"received(\b(.*)(\s+from))");
            var regCash = new Regex(string.Format(@"{0}(\b(.*)(\s+{1}))", vStatus[0], vStatus[1]));
            var regBalance = new Regex(@"balance is(\s\b(.*)\D)");
            var regDate = new Regex(@"on(\b(.*)(\s+at))");
            var regRName = new Regex(string.Format(@"{0}(\b(.*)(\s)\b(on|in)(\s.*\w\s))", vStatus[2]));

            var regPayPall = new Regex(@"via(\b(.*)(\s)\b(on|in)(\s.*\w\s))");

            var cashAmountBefore = regCash.Matches(szvBody);
            var cashBalanceBefore = regBalance.Matches(szvBody);
            var dateBefore = regDate.Matches(szvBody);
            var rNameBefore = regRName.Matches(szvBody);

            var rPayPallBefore = regPayPall.Matches(szvBody);
            bool bPayPall = rPayPallBefore.Count.Equals(0);

            cashAmountAfter = cashAmountBefore[0].Value;
            SzACashAmount = cashAmountAfter.Split(' ');

            cashBalanceAfter = cashBalanceBefore[0].Value;
            SzACashBalance = cashBalanceAfter.Split(' ');

            try
            {
                dateAfter = dateBefore[0].Value;
            }
            catch (Exception ex)
            {

                dateAfter = "on 01/1/97 at";
            }

            try
            {
                rNameAfter = rNameBefore[0].Value;
            }
            catch (Exception ex)
            {

                rNameAfter = "Null Null Null Null";
            }
            SzADate = dateAfter.Split(' ');

            
            SzARName = rNameAfter.Split(' ');

            vmessage.RName = SzARName[1] + " " + SzARName[2] + " " + SzARName[3];
            vmessage.Balance = SzACashBalance[2];
            vmessage.Code = wordsArray[0];
            vmessage.TransactionStatus = szvID;
            vmessage.RDate = SzADate[1];
            vmessage.CashAmount = SzACashAmount[1];
            

            switch (szvID)
            {
                case "Paybill":
                    vmessage.RAccNo = SzARName[1];
                    break;
                case "airtime":
                    if (SzARName[1] == "on")
                    {
                        vmessage.RName = "airtime";
                    }
                    break;
                case "sent":
                    break;
                case "received":
                    if (bPayPall)
                    {
                        break;
                    }
                    vmessage.Quota = wordsArray[13];

                    break;
            }

            xml_prop.Add(vmessage);

            return vStatus[0];
        }
        private string FulizaLoanFormater(string szvBody,
                                    X_XMLProperties vmessage,
                                    string[] wordsArray,
                                    string[] vStatus,
                                    string szvID)
        {
                                                                                        string[]? SzAFulizaAmount = null;
                                                                                        string[]? SzAFulizaCharge = null;
                                                                                        string[]? SzAFulizaDebt = null;
                                                                                        string[]? SzAFulizaLimit = null;

            List<X_XMLProperties> xml_prop = new List<X_XMLProperties>();
            
            //bool bFulizaCheck = wordsArray[0].StartsWith(szvID);

            bool extraCheck = new Regex(@"Ksh(\b(.*)(\s)\b(Fee))").ToString().Contains("Ksh");

            if (extraCheck)
            {
                var regFulizaAmount = new Regex(@"Ksh(\b(.*)(\s)\b(Fee))");
                var regFulizaCharge = new Regex(@"charged (\b(.*)(\s)\b(Total))");
                var regFulizaDebt = new Regex(@"outstanding amount is (\b(.*)(\s)\b(due))");

                var cashFulizaAmountBefore = regFulizaAmount.Matches(szvBody);
                var cashFulizaChargeBefore = regFulizaCharge.Matches(szvBody);
                var cashFulizaDebtBefore = regFulizaDebt.Matches(szvBody);

                string cashFulizaAmountAfter = cashFulizaAmountBefore[0].Value;
                SzAFulizaAmount = cashFulizaAmountAfter.Split(' ');

                string cashFulizaChargeAfter = cashFulizaChargeBefore[0].Value;
                SzAFulizaCharge = cashFulizaChargeAfter.Split(' ');

                string cashFulizaDebtAfter = cashFulizaDebtBefore[0].Value;
                SzAFulizaDebt = cashFulizaDebtAfter.Split(' ');

                vmessage.Code = wordsArray[0];
                vmessage.TransactionStatus = szvID;
                vmessage.FulizaAmount = SzAFulizaAmount[0];
                vmessage.FulizaCharge = SzAFulizaCharge[0];
                vmessage.FulizaBorrowed = SzAFulizaDebt[0];
            }
            if (!extraCheck)
            {
                var regFulizaLimit = new Regex(@"M-PESA limit is (\b(.*)(\s)\b(M-PESA))");
                var regFulizaAmount = new Regex(@"Confirmed(\b(.*)(\s)\b(from))");

                var cashFulizaAmountBefore = regFulizaAmount.Matches(szvBody);
                var cashFulizaLimitBefore = regFulizaLimit.Matches(szvBody);

                string cashFulizaAmountAfter = cashFulizaAmountBefore[0].Value;
                SzAFulizaAmount = cashFulizaAmountAfter.Split(' ');

                string cashFulizaLimitAfter = cashFulizaLimitBefore[0].Value;
                SzAFulizaLimit = cashFulizaLimitAfter.Split(' ');


                vmessage.Code = wordsArray[0];
                vmessage.TransactionStatus = szvID;
                vmessage.FulizaLimit = SzAFulizaLimit[4];
                vmessage.FulizaAmount = SzAFulizaAmount[2];
            }

            xml_prop.Add(vmessage);

            return vStatus[0];
        }
    }
}
