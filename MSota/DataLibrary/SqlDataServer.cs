﻿using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using MSota.ExtensibleMarkupAtLarge;
using System.Collections.Generic;

namespace MSota.DataLibrary
{
    public class SqlDataServer : ISqlDataServer
    {
        private ISQLDataAccess _dataAccess;

        public SqlDataServer(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public void PostData(List<IXmlProps> x_vprop)
        {
            AddStatisticsToDb(x_vprop);
        }
        public void AddStatisticsToDb(List<IXmlProps> x_vprop)
        {
            string szSQL = string.Empty;

            foreach (IXmlProps prop in x_vprop)
            {
                    szSQL = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                    + "'" + prop.szCode + "',"
                                    + "'" + prop.szDate + "',"
                                    + "'" + prop.szRName + "',"
                                    + "'" + prop.szRPhoneNo + "',"
                                    //+ "'" + prop.szRecepientDate + "',"
                                    + "'" + prop.szRAccNo + "',"
                                    + "'" + prop.dCashAmount + "',"
                                    + "'" + prop.dBalance + "',"
                                    + "'" + prop.szProtocol + "',"
                                    + "'" + prop.szPayBill_TillNo + "',"
                                    + "'" + prop.szTransactionCost + "',"
                                    + "'" + prop.szAddress + "',"
                                    + "'" + prop.szType + "',"
                                    + "'" + prop.szSubject + "',"
                                    + "'" + prop.szBody + "',"
                                    + "'" + prop.szToa + "',"
                                    + "'" + prop.szSc_toa + "',"
                                    + "'" + prop.szService_center + "',"
                                    + "'" + prop.szRead + "',"
                                    + "'" + prop.szLocked + "',"
                                    + "'" + prop.szDate_sent + "',"
                                    + "'" + prop.szStatus + "',"
                                    + "'" + prop.szSub_id + "',"
                                    + "'" + prop.szReadable_date + "',"
                                    + "'" + prop.szQuota + "',"
                                    + "'" + prop.dFulizaLimit + "',"
                                    + "'" + prop.dFulizaBorrowed + "',"
                                    + "'" + prop.dCharges + "',"
                                    + "'" + prop.dFulizaAmount + "',"
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
        public List<DL_XMLDataModel> LoadTransactionStatistics
            => _dataAccess.LoadData<DL_XMLDataModel>("SELECT "

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

                                        + "FROM[DB_MSota].[dbo].[Ms_Transactions]        ");
    }
}