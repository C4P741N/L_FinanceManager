using System.Text.RegularExpressions;

namespace MSota.BaseFormaters
{
    public interface IFortmaterAtLarge
    {
        string ConvertToString(double vValue);
        string ConvertToString(string vValue);
        public string GlobalRNameGetter(string szvBody, string [] SzStatus);
        public string GlobalAccNoAndPhoneNoGetter(string szvBody);
        public double GlobalCashGetter(string szvBody);
        public string [] GlobalCashGetterArray(string szvBody);
        string[] BodyToValueArray(string szvBody, Regex RBody);
        string GetUniqueKey();
        double CashConverter(string vValue);
        DateTime DateConvertionFromLong(long lvDate);
        long DateConvertionFromLongToTicksVal(long lvDate);
        long DateConvertionToLongTicks(long lvDate);
        DateTime DateFormat_dMyy_Convertion(string szvDate);
        string[] DateSplitByBackSlash(string value);
        string GetAccountNumber(string szvBody);
        string GetAirtimeSentToNumber(string szvBody);
        double GetCashAmount(string szvBody, string[] vSzAStatus);
        double GetCashBalance(string szvBody);
        double GetCharges(string szvBody);
        string GetContactName(string szvBody, string szvID, string[] SzAvStatus);
        string GetDate(string szvBody);
        string GetKCBContactName(string szvBody);
        string GetMpesaCode(string szvBody);
        string GetPayBillNo(string szvBody);
        string GetPhoneNumber(string szvBody, string szvID, string[] SzAvStatus);
        string RNameCreatorFromAccNo(string szAccNo);
        string SpecialCharactersRemover(string szValue);
        string StringFormaterToProperCase(string szvValue);
        string StringSplitAndJoin(string szValue);
    }
}