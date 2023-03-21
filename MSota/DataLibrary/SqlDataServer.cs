using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using MSota.ExtensibleMarkupAtLarge;
using System.Collections.Generic;
using System.Globalization;
using MSota.BaseFormaters;

namespace MSota.DataLibrary
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

        public List<DL_XMLDataModel> LoadTransactionStatistics()
        {
            string szSQL = "SELECT "

                        + " [Code]                  AS Code              " + Environment.NewLine
                        + ",[Code_ID]               AS Code_ID           " + Environment.NewLine
                        + ",[M_Date]                AS Date              " + Environment.NewLine
                        + ",[M_RecepientPhoneNo]    AS RecepientPhoneNo  " + Environment.NewLine
                        + ",[M_CashAmount]          AS CashAmount        " + Environment.NewLine
                        + ",[M_Balance]             AS Balance           " + Environment.NewLine
                        + ",[M_PayBill_TillNo]      AS PayBill_TillNo    " + Environment.NewLine
                        + ",[M_TransactionCost]     AS TransactionCost   " + Environment.NewLine
                        + ",[M_Quota]               AS Quota             " + Environment.NewLine
                        + ",[M_FulizaLimit]         AS FulizaLimit       " + Environment.NewLine
                        + ",[M_FulizaBorrowed]      AS FulizaBorrowed    " + Environment.NewLine
                        + ",[M_FulizaCharge]        AS FulizaCharge      " + Environment.NewLine
                        + ",[M_FulizaAmount]        AS FulizaAmount      " + Environment.NewLine

                        + "FROM [Ms_DataCollector].[dbo].[Ms_Transactions]        ";

            return _dataAccess.LoadData<DL_XMLDataModel>(szSQL);
        } 
    }
}
