using System.Text.RegularExpressions;

namespace MSota.BaseFormaters
{
    public interface IFortmaterAtLarge
    {
        string[] BodyToValueArray(string szvBody, Regex RBody);
        string GetUniqueKey();
        string CashConverter(string vValue);
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