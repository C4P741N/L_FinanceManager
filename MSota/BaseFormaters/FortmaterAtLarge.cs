using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MSota.BaseFormaters
{
    public class FortmaterAtLarge : IFortmaterAtLarge
    {
        //public string[] GlobalBodyToValueArray(string szvBody, Regex RBody)
        //{
        //    string szAmountAfter = string.Empty;
        //    string[] SzAmount;

        //    var AmountBefore = RBody.Matches(szvBody);

        //    if (AmountBefore.Count.Equals(0)) return (SzAmount = szAmountAfter.Split(' '));

        //    //szAmountAfter = AmountBefore;

        //    //SzAmount = szAmountAfter.Split(' ');

        //    //SzAmount = SzAmount.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        //    return AmountBefore;
        //}
        private string StringToTitleCase(string szName)
        {
            var name = szName.Substring(0, 1).ToUpper() + szName.Substring(1).ToLower();

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }
        public string GlobalRNameGetter(string szvBody, string[] status)
        {
            string[] dFin = new string[] { };
            string szOut = string.Empty;

            var regName = new Regex($@"{status[0]}\s+{status[1]}\s+(\w+\s+\w+)");

            if (status[2] == "withdraw")
                regName = new Regex(@"(?<=from\s)(.*?)(?=\sNew)");

            var rNameBefore = regName.Matches(szvBody);

            bool bIsBeforeEmpty = rNameBefore.Count.Equals(0);

            if (bIsBeforeEmpty == true) return string.Empty;

            dFin = BodyToValueArray(szvBody, regName);

            if (status[2] == "withdraw")
                return szOut = StringToTitleCase(rNameBefore.Select(x => x.ToString()).ToArray()[0]);
            if (status[1] == "for")
                return szOut = StringToTitleCase(dFin[1]);
            if (status[1] == "from")
                return szOut = StringToTitleCase(dFin[1] + " " + dFin[2]);
            if (status[0] == "sent" || status[0] == "paid")
                return szOut = StringToTitleCase(dFin[2] + " " + dFin[3]);

            return StringToTitleCase(szOut);
        }
        public string GlobalAccNoAndPhoneNoGetter(string szvBody)
        {
            var regAccNo = new Regex(@"\b\d{10}\b");
            var rAccNoBefore = regAccNo.Matches(szvBody);

            bool bIsBeforeEmpty = rAccNoBefore.Count.Equals(0);

            if (bIsBeforeEmpty == true) return string.Empty;

            return BodyToValueArray(szvBody, regAccNo)[0];
        }
        public double GlobalCashGetter(string szvBody)
        {
            string[] SzFin = new string[] {};
            string[] SzResult = new string[5];
            double dResult = 0;
            System.Globalization.CultureInfo CultureInfo = new CultureInfo("en-GB");

            var regCash = new Regex(@"\d+(?:,\d+)*\.\d{2}");
            var rCashBefore = regCash.Matches(szvBody);

            bool bIsBeforeEmpty = rCashBefore.Count.Equals(0);

            if (bIsBeforeEmpty == true) return dResult;

            //var tes0 = BodyToValueArray(szvBody, regCash);

            SzResult = rCashBefore.Select(x => x.Value).ToArray();

            dResult = Convert.ToDouble(SzResult[0], CultureInfo);

            return dResult;
        }

        public string [] GlobalCashGetterArray(string szvBody)
        {
            string[] SzFin = new string[] { };
            string[] SzResult = new string[5];
            double dResult = 0;

            var regCash = new Regex(@"\d+(?:,\d+)*\.\d{2}");
            var rCashBefore = regCash.Matches(szvBody);

            bool bIsBeforeEmpty = rCashBefore.Count.Equals(0);

            if (bIsBeforeEmpty == true) return SzFin;

            //var tes0 = BodyToValueArray(szvBody, regCash);

            SzResult = rCashBefore.Select(x => x.Value).ToArray();

            return SzResult;
        }
        public string GetUniqueKey()
        {
            StringBuilder builder = new StringBuilder();

            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));

            return builder.ToString().ToUpper();
        }
        public double GetCharges(string szvBody)
        {
            bool bIsBeforeBeforeEmpty = false;
            double dCharges = 0;

            var regCostings = new Regex(@"Transaction cost,(\s\b(.*)\D)");

            var rCostingsBefore = regCostings.Matches(szvBody);

            bIsBeforeBeforeEmpty = rCostingsBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return dCharges;

            dCharges = Convert.ToDouble(CashConverter(BodyToValueArray(szvBody, regCostings)[2]));

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

            return StringToTitleCase(szContactName);
        }

        public string GetContactName(string szvBody,
                                     string szvID,
                                     string[] SzAvStatus)
        {
            string szNameAfter = string.Empty;
            string szContactName = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            var regRName = new Regex(string.Format(@"{0}(\b(.*)(\s)\b(on|in)(\s.*\w\s))", SzAvStatus[2]));

            var rNameBefore = regRName.Matches(szvBody);

            bIsBeforeBeforeEmpty = rNameBefore.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

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
                        szContactName = "Airtime Purchase";
                        break;
                    }
                    if (BodyToValueArray(szvBody, regRName)[1] == "for")
                    {
                        szContactName = "Airtime Sent";
                        break;
                    }
                    szContactName = BodyToValueArray(szvBody, regRName)[1] + " " + BodyToValueArray(szvBody, regRName)[2];

                    if (szContactName.Contains("KCB"))
                    {
                        szContactName = BodyToValueArray(szvBody, regRName)[1];
                    }

                    break;
                case "withdraw":
                    szContactName = RNameCreatorFromAccNo(BodyToValueArray(szvBody, regRName)[1]);
                    break;
            }
            return szContactName;
        }

        public string RNameCreatorFromAccNo(string szAccNo)
        {
            string szRNameAccNo = string.Empty;

            if (szAccNo == null) return string.Empty;

            szRNameAccNo = "account " + szAccNo;

            return szRNameAccNo;
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

                    if (bIsBeforeBeforeEmpty == true) return string.Empty;

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
            if (string.IsNullOrEmpty(szValue)) return string.Empty;

            string[] SzASplitVal = szValue.Split(' ');

            szValue = SpecialCharactersRemover(SzASplitVal[0] + SzASplitVal[1]);

            return szValue;
        }

        public string SpecialCharactersRemover(string szValue)
        {
            if (string.IsNullOrEmpty(szValue)) return string.Empty;

            szValue = Regex.Replace(szValue, "[^a-zA-Z0-9% ]", string.Empty);

            return szValue;
        }

        public string GetAirtimeSentToNumber(string szvBody)
        {
            string szAirtimeNo = string.Empty;
            bool bIsBeforeBeforeEmpty = false;

            var regRAirtimeNo = new Regex(@"for(\b(.*)(\s)\b(on|in))");

            var rRAirtimeNo = regRAirtimeNo.Matches(szvBody);

            bIsBeforeBeforeEmpty = rRAirtimeNo.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

            szAirtimeNo = BodyToValueArray(szvBody, regRAirtimeNo)[1];

            return szAirtimeNo;
        }

        public string GetAccountNumber(string szvBody)
        {
            string szPhoneOrAccNo = string.Empty;
            string szIsPhoneOrAccNo;
            bool bIsBeforeBeforeEmpty;
            int ignored;

            if (szvBody.Contains("Safaricom Limited") ||
                szvBody.Contains("Safaricom Offers")) return string.Empty;

            var regRAccountNo = new Regex(@"(for account (.*))|(from KCB )((.*)(\s+)on)");

            var rRAccountNo = regRAccountNo.Matches(szvBody);

            bIsBeforeBeforeEmpty = rRAccountNo.Count.Equals(0);

            if (bIsBeforeBeforeEmpty == true) return string.Empty;

            szPhoneOrAccNo = BodyToValueArray(szvBody, regRAccountNo)[2];
            szIsPhoneOrAccNo = BodyToValueArray(szvBody, regRAccountNo)[3];

            bool isInt = int.TryParse(szIsPhoneOrAccNo, out ignored);

            if (isInt)
            {
                szPhoneOrAccNo = szPhoneOrAccNo + szIsPhoneOrAccNo;
            }

            return szPhoneOrAccNo;
        }

        private string xx_PhoneNoConverterTo07(string szNumber)
        {
            string szFormatedNumber = string.Empty;

            if (szNumber == string.Empty) return string.Empty;

            szFormatedNumber = szNumber.Remove(0, 3);

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

            if (szvBody == string.Empty) return string.Empty;

            var regCode = new Regex(@"\s(Reference:(.*))|ref\s(\b(.*)\s)");

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

            if (pos == -1) return szvValue;

            szvValue = szvValue.Substring(0, pos + 0);

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

            if (lvDate == 0) return date;

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date;
        }

        public string StringFormaterToProperCase(string szvValue)
        {
            if (string.IsNullOrEmpty(szvValue)) return string.Empty;

            string Value = System.Threading.Thread.CurrentThread
                                                .CurrentCulture.TextInfo.ToTitleCase(szvValue.ToLower());

            return Value;
        }

        public long DateConvertionToLongTicks(long lvDate)///************
        {
            DateTime date = new DateTime();
            long unixDate = lvDate;

            if (lvDate == 0) return lvDate;

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return lvDate;
        }

        public long DateConvertionFromLongToTicksVal(long lvDate)
        {
            DateTime date = new DateTime();
            long unixDate = lvDate;

            if (lvDate == 0) return unixDate;

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            date = start.AddMilliseconds(unixDate).ToLocalTime();

            long lDate = date.Ticks;

            return lDate;
        }

        public DateTime DateFormat_dMyy_Convertion(string szvDate)
        {
            DateTime dt = new DateTime();
            string szDateFormated;

            if (string.IsNullOrEmpty(szvDate)) return dt;

            string szDay = DateSplitByBackSlash(szvDate)[0];
            string szMonth = DateSplitByBackSlash(szvDate)[1];
            string szYear = DateSplitByBackSlash(szvDate)[2];

            szDateFormated = szDay + "/" + szMonth + "/" + szYear;

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

        public double CashConverter(string vValue)
        {

            if (string.IsNullOrEmpty(vValue)) return 0;

            System.Globalization.CultureInfo dAmountCultureInfo = new CultureInfo("en-GB");

            string strFinValue = string.Empty;
            double dFiValue = 0;
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
            dFiValue = Convert.ToDouble(strFinValue, dAmountCultureInfo);

            return dFiValue;
        }
        public string ConvertToString(string vValue)
        {

            if (string.IsNullOrEmpty(vValue)) return string.Empty;

            System.Globalization.CultureInfo dAmountCultureInfo = new CultureInfo("en-GB");

            string szFiValue = Convert.ToString(vValue, dAmountCultureInfo);

            return szFiValue;
        }
        public string ConvertToString(double vValue)
        {

            if (vValue == 0) return string.Empty;

            System.Globalization.CultureInfo dAmountCultureInfo = new CultureInfo("en-GB");

            string szFiValue = Convert.ToString(vValue, dAmountCultureInfo);

            return szFiValue;
        }

    }
}
