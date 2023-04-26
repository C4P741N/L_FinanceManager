using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using MSota.ExtensibleMarkupAtLarge;
using System.Collections.Generic;
using System.Globalization;
using MSota.BaseFormaters;
using System.Text.RegularExpressions;
using MSota.Models;
using Calendar = MSota.Models.Calendar;

namespace MSota.DataServer
{
    public class SqlDataServer : ISqlDataServer
    {
        private ISQLDataAccess _dataAccess;
        IFortmaterAtLarge _fortmater;

        public SqlDataServer(ISQLDataAccess dataAccess, IFortmaterAtLarge fortmater)
        {
            _dataAccess = dataAccess;
            _fortmater = fortmater;
        }

        public void PostData(List<IXmlProps> x_vprop)
        {
            AddStatisticsToDb(x_vprop);
        }
        public void AddStatisticsToDb(List<IXmlProps> x_vprop)
        {
            string szSQL = string.Empty;
            System.Globalization.CultureInfo dAmountCultureInfo = new CultureInfo("en-GB");

            foreach (IXmlProps prop in x_vprop)
            {
                szSQL = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                + "'" + prop.szCode + "'," + Environment.NewLine
                                + "'" + prop.szDate + "'," + Environment.NewLine
                                + "'" + prop.szRName + "'," + Environment.NewLine
                                + "'" + prop.szRPhoneNo + "'," + Environment.NewLine
                                //+ "'" + prop.szRecepientDate + "'," + Environment.NewLine
                                + "'" + prop.szRAccNo + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dCashAmount) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dBalance) + "'," + Environment.NewLine
                                + "'" + prop.szProtocol + "'," + Environment.NewLine
                                + "'" + prop.szPayBill_TillNo + "'," + Environment.NewLine
                                + "'" + prop.szTransactionCost + "'," + Environment.NewLine
                                + "'" + prop.szAddress + "'," + Environment.NewLine
                                + "'" + prop.szType + "'," + Environment.NewLine
                                + "'" + prop.szSubject + "'," + Environment.NewLine
                                + "'" + prop.szBody + "'," + Environment.NewLine
                                + "'" + prop.szToa + "'," + Environment.NewLine
                                + "'" + prop.szSc_toa + "'," + Environment.NewLine
                                + "'" + prop.szService_center + "'," + Environment.NewLine
                                + "'" + prop.szRead + "'," + Environment.NewLine
                                + "'" + prop.szLocked + "'," + Environment.NewLine
                                + "'" + prop.szDate_sent + "'," + Environment.NewLine
                                + "'" + prop.szStatus + "'," + Environment.NewLine
                                + "'" + prop.szSub_id + "'," + Environment.NewLine
                                + "'" + prop.szReadable_date + "'," + Environment.NewLine
                                + "'" + prop.szQuota + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaLimit) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaBorrowed) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dCharges) + "'," + Environment.NewLine
                                + "'" + _fortmater.ConvertToString(prop.dFulizaAmount) + "',"
                                + "'" + prop.szUniqueKey + "'"

                                ;

                _dataAccess.SaveData(szSQL);
            }
            _CopyAndSaveCollectionsToRecepientsAndTransactions();
        }

        private void _CopyAndSaveCollectionsToRecepientsAndTransactions()
        {
            string szSQL;

            szSQL = "EXECUTE RecepientsCopier";

            _dataAccess.SaveData(szSQL);

            szSQL = "EXECUTE TransactionsCopier";

            _dataAccess.SaveData(szSQL);
        }

        public List<TransactionModel> LoadTransactionStatistics(Calendar cal)
        {
            string szSQL = "SELECT" + Environment.NewLine

                           + "R.[M_RecepientName]               AS RecepientName" + Environment.NewLine
                           + ",R.[M_UniqueID]                   AS RecepientID" + Environment.NewLine
                           + ",SUM(T.M_CashAmount)              AS TransactionAmount" + Environment.NewLine
                           + ",T.[M_Quota]                      As TranactionQuota" + Environment.NewLine

                           + "FROM[Ms_DataCollector].[dbo].[Ms_Recepients] R" + Environment.NewLine

                           + "JOIN[Ms_DataCollector].[dbo].[Ms_Transactions] T" + Environment.NewLine
                           + "ON R.M_UniqueID = T.M_UniqueID" + Environment.NewLine

                           + " WHERE [M_Date] BETWEEN '" + cal .from + "' AND '" + cal.to + "'" + Environment.NewLine
                           + "AND T.[M_Quota] != 'AccountDeposit'" + Environment.NewLine

                           + "GROUP BY R.[M_RecepientName], T.[M_Quota], R.[M_UniqueID]"; 

            return _dataAccess.LoadData<TransactionModel>(szSQL);
        } 

        public List<FactionsModel> LoadFactionsStatistics(Calendar cal)
        {
            string szSQL = "SELECT" + Environment.NewLine

                            + " [M_Quota] AS GroupName" + Environment.NewLine
                            + " , SUM([M_CashAmount]) AS GroupTotal" + Environment.NewLine


                            + "  FROM[Ms_DataCollector].[dbo].[Ms_Transactions]" + Environment.NewLine

                            + " WHERE [M_Date] BETWEEN '" + cal.from + "' AND '" + cal.to + "'" + Environment.NewLine
                            + "AND [M_Quota] != 'AccountDeposit'" + Environment.NewLine

                            + "  GROUP BY[M_TransactionCost],[M_Quota]";


            return _dataAccess.LoadData<FactionsModel>(szSQL);
        }
    }
}
