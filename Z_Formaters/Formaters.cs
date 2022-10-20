﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Z_Formaters
{
    public class Formaters
    {
        public double GetCharges(string szvBody)
        {
            bool bIsBeforeBeforeEmpty = false;
            double dCharges = 0;

            var regCostings = new Regex(@"Transaction cost,(\s\b(.*)\D)");

            var rCostingsBefore = regCostings.Matches(szvBody);

            bIsBeforeBeforeEmpty = rCostingsBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == false)
            {
                dCharges = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regCostings)[2]));
            }

            return dCharges;
        }

        public string GetKCBContactName(string szvBody)
        {
            bool bIsBeforeBeforeEmpty = false;
            string szContactName = string.Empty;

            var regContact = new Regex(@"(?:(([^-]*)\s)has)");

            var rContact = regContact.Matches(szvBody);

            bIsBeforeBeforeEmpty = rContact.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;
            
            szContactName = BodyToValueArray(szvBody, regContact)[0] + " " + BodyToValueArray(szvBody, regContact)[1];
            
            return szContactName;
        }

        public string GetContactName(string szvBody,
                                     string szvID,
                                     string[] SzAvStatus)
        {
            string szNameAfter = string.Empty;
            string szContactName = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            while (true)
            {
                var regRName = new Regex(string.Format(@"{0}(\b(.*)(\s)\b(on|in)(\s.*\w\s))", SzAvStatus[2]));
                
                var rNameBefore = regRName.Matches(szvBody);
                
                bIsBeforeBeforeEmpty = rNameBefore.Count.Equals(0);

                if (bIsBeforeBeforeEmpty == true)
                {
                    break;
                }

                switch (szvID)
                {
                    case "paybill":
                        szContactName = BodyToValueArray(szvBody, regRName)[0] + " " + BodyToValueArray(szvBody, regRName)[1];
                        break;
                    case "airtime":
                    case "paid":
                    case "sent":
                    case "received":
                        if (BodyToValueArray(szvBody, regRName)[1] == "on")
                        {
                            szContactName = "Airtime Bought";
                            //szvID = "paid";
                            break;
                        }
                        if (BodyToValueArray(szvBody, regRName)[1] == "for")
                        {
                            szContactName = "Airtime Sent";
                            //szvID = "sent";
                            break;
                        }
                        szContactName = BodyToValueArray(szvBody, regRName)[1] + " " + BodyToValueArray(szvBody, regRName)[2];

                        if (szContactName.Contains("KCB"))
                        {
                            szContactName = BodyToValueArray(szvBody, regRName)[1];
                        }

                        break;
                    case "withdraw":
                        szContactName = "Account" + " " + BodyToValueArray(szvBody, regRName)[1];
                        break;
                }

                break;
            }
            return szContactName;
        }

        public string GetPhoneNumber(string szvBody,
                                     string szvID,
                                     string[] SzAvStatus)
        {
            string szPhoneNo = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            while (true)
            {

                var regRPhoneNo254 = new Regex(@"254((.*)(\s+)on(.*)(\s+)at)");
                var regRPhoneNo07 = new Regex(@"\s07((.*)(\s+on))");

                var rPhoneNo254Before = regRPhoneNo254.Matches(szvBody);
                var rPhoneNo07Before = regRPhoneNo07.Matches(szvBody);

                if (szvBody.Contains("+254"))
                {

                    bIsBeforeBeforeEmpty = rPhoneNo254Before.Count.Equals(0);

                    if (bIsBeforeBeforeEmpty == true)
                    {
                        break;
                    }

                    szPhoneNo = xx_PhoneNoConverterTo07(BodyToValueArray(szvBody, regRPhoneNo254)[0]);

                    break;
                }

                bIsBeforeBeforeEmpty = rPhoneNo07Before.Count.Equals(0);

                if (bIsBeforeBeforeEmpty == false)
                {
                    szPhoneNo = BodyToValueArray(szvBody, regRPhoneNo07)[0];
                    break;
                }

                if (szvID == "airtime")
                {
                    szPhoneNo = xx_PhoneNoConverterTo07(GetAirtimeSentToNumber(szvBody));
                    break;
                }

                szPhoneNo = GetAccountNumber(szvBody);

                if (szvBody.Contains("PayPal") || szvID == "paid" || szvID == "withdraw" || szPhoneNo == "sent")
                {
                    szPhoneNo = StringSplitAndJoin(GetContactName(szvBody, szvID, SzAvStatus));
                    break;
                }

                break;
            }
            return szPhoneNo;
        }

        public string StringSplitAndJoin(string szValue)
        {
            if (szValue != string.Empty)
            {
                string[] SzASplitVal = szValue.Split(' ');

                szValue = SzASplitVal[0] + SzASplitVal[1];
            }

            return szValue;
        }

        public string GetAirtimeSentToNumber(string szvBody)
        {
            string szAirtimeNo = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            while (true)
            {
                var regRAirtimeNo = new Regex(@"for(\b(.*)(\s)\b(on|in))");

                var rRAirtimeNo = regRAirtimeNo.Matches(szvBody);

                bIsBeforeBeforeEmpty = rRAirtimeNo.Count.Equals(0);

                if (bIsBeforeBeforeEmpty == false)
                {
                    szAirtimeNo = BodyToValueArray(szvBody, regRAirtimeNo)[1];
                }
                break;
            }
            return szAirtimeNo;
        }

        public string GetAccountNumber(string szvBody)
        {
            string szPhoneNo = string.Empty;
            string szIsPhoneNo;
            bool bIsBeforeBeforeEmpty;
            int ignored;

            while (true)
            {
                if (szvBody.Contains("Safaricom Limited") ||
                    szvBody.Contains("Safaricom Offers"))
                {
                    break;
                }

                var regRAccountNo = new Regex(@"account((.*)(\s+)on)");

                var rRAccountNo = regRAccountNo.Matches(szvBody);

                bIsBeforeBeforeEmpty = rRAccountNo.Count.Equals(0);

                if (bIsBeforeBeforeEmpty == false)
                {
                    szPhoneNo = BodyToValueArray(szvBody, regRAccountNo)[1];
                    szIsPhoneNo = BodyToValueArray(szvBody, regRAccountNo)[2];

                    bool isInt = int.TryParse(szIsPhoneNo, out ignored);

                    if (isInt)
                    {
                        szPhoneNo = szPhoneNo + szIsPhoneNo;
                    }
                }

                break;
            }
            return szPhoneNo;
        }

        private string xx_PhoneNoConverterTo07(string szNumber)
        {
            string szFormatedNumber = string.Empty;

            if (szNumber != string.Empty)
            {
                szFormatedNumber = szNumber.Remove(0, 3);

                return "0" + szFormatedNumber;
            }

            return "0" + szFormatedNumber;
        }

        public double GetCashBalance(string szvBody)
        {
            double dBalance = 0;
            bool bIsBeforeBeforeEmpty = false;

            var regBalance = new Regex(@"balance is(\s\b(.*)\D)");

            var rBalanceBefore = regBalance.Matches(szvBody);

            bIsBeforeBeforeEmpty = rBalanceBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return dBalance;

            dBalance = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regBalance)[2]));

            return dBalance;
        }

        public double GetCashAmount(string szvBody,
                                    string[] vSzAStatus)
        {
            double dBalance = 0;
            bool bIsBeforeBeforeEmpty = false;

            var regCash = new Regex(string.Format(@"{0}(\b(.*)(\s+{1}))", vSzAStatus[0], vSzAStatus[1]));

            var rCashBefore = regCash.Matches(szvBody);

            bIsBeforeBeforeEmpty = rCashBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return dBalance;

            dBalance = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regCash)[1]));

            return dBalance;
        }

        public string GetPayBillNo(string szvBody)
        {
            string szPayBillNo = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            if (szvBody == string.Empty) return szPayBillNo;

            var regPayBill = new Regex(@"\sBill\s(\b(.*)(\s+for))");

            var rayBillBefore = regPayBill.Matches(szvBody);

            bIsBeforeBeforeEmpty = rayBillBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

            szPayBillNo = BodyToValueArray(szvBody, regPayBill)[1];

            return szPayBillNo;
        }

        public string GetMpesaCode(string szvBody)
        {
            string szCode = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            if (szvBody == string.Empty) return string.Empty; ;

            var regCode = new Regex(@"\sref\s(\b(.*)(\s+))");

            var rCodeBefore = regCode.Matches(szvBody);

            bIsBeforeBeforeEmpty = rCodeBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

            szCode = CheckAndRetrieveValuePastFullStop(BodyToValueArray(szvBody, regCode)[1]);

            return szCode;
        }

        private string CheckAndRetrieveValuePastFullStop(string szvValue)
        {
            if (szvValue == string.Empty) return string.Empty;

            var pos = szvValue.IndexOf('.');

            if (pos == -1) return string.Empty;

            szvValue = szvValue.Substring(0, pos - 1);

            return szvValue;
        }

        public string GetDate(string szvBody)
        {
            string szDateBefore = string.Empty;
            string szDateAfter = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            var regDate = new Regex(@"\son(\b(.*)(\s+at))");

            var rDateBefore = regDate.Matches(szvBody);

            bIsBeforeBeforeEmpty = rDateBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

            szDateBefore = BodyToValueArray(szvBody, regDate)[1];

            szDateAfter = Convert.ToString(DateFormat_dMyy_Convertion(szDateBefore));

            return szDateAfter;
        }

        public DateTime DateConvertionFromLong(long lvDate)
        {
            DateTime date = new DateTime();
            long unixDate = lvDate;

            if(lvDate == 0) return date;

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date;
        }

        public DateTime DateFormat_dMyy_Convertion(string szvDate)
        {
            DateTime dt = new DateTime();
            string szDateFormated;

            if (string.IsNullOrEmpty(szvDate)) return dt;

            string szDay = DateSplitByBackSlash(szvDate)[0];
            string szMonth = DateSplitByBackSlash(szvDate)[1];
            string szYear = DateSplitByBackSlash(szvDate)[2];

            szDateFormated = szMonth + "/" + szDay + "/" + szYear;

            dt = Convert.ToDateTime(szDateFormated);

            return dt;
        }

        public string[] DateSplitByBackSlash(string value)
        {
            string[]? test = null;

            if (string.IsNullOrEmpty(value)) return null;

            test = value.Split('/');

            return test;
        }

        //public string Left(string value, int maxLength)
        //{
        //    if (string.IsNullOrEmpty(value)) return value;
        //    maxLength = Math.Abs(maxLength);

        //    return (value.Length <= maxLength
        //           ? value
        //           : value.Substring(0, maxLength)
        //           );
        //}

        public string[] BodyToValueArray(string szvBody, Regex RBody)
        {
            string szAmountAfter = string.Empty;
            string[] SzAmount;

            var AmountBefore = RBody.Matches(szvBody);

            if (AmountBefore.Count.Equals(0)) return (SzAmount = szAmountAfter.Split(' '));

            szAmountAfter = AmountBefore[0].Value;

            SzAmount = szAmountAfter.Split(' ');

            SzAmount = SzAmount.Where(x => !string.IsNullOrEmpty(x)).ToArray();

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