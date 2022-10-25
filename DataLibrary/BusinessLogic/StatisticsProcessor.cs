using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using ExtensibleMarkupAtLarge;
using System.Web.Http;
using Microsoft.Data.SqlClient;

namespace DataLibrary.BusinessLogic
{
    public static class StatisticsProcessor
    {
        private static List<X_XMLProperties> x_prop = null;
        private static HttpResponseException exception = null;
        public static void AddStatisticsToDb(List<X_XMLProperties> x_vprop)
        {
                                                                                            string szSQL = string.Empty;

            x_prop = x_vprop;

            foreach (X_XMLProperties prop in x_prop)
            {
                try
                {
                    szSQL = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                    + "'" + prop.Code + "',"
                                    + "'" + prop.szDate + "',"
                                    + "'" + prop.RName + "',"
                                    + "'" + prop.RPhoneNo + "',"
                                    + "'" + prop.RDate + "',"
                                    + "'" + prop.RAccNo + "',"
                                    + "'" + prop.CashAmount + "',"
                                    + "'" + prop.Balance + "',"
                                    + "'" + prop.szProtocol + "',"
                                    + "'" + prop.PayBill_TillNo + "',"
                                    + "'" + prop.TransactionCost + "',"
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
                                    + "'" + prop.RAccNo + "',"
                                    + "'" + prop.Quota + "',"
                                    + "'" + prop.FulizaLimit + "',"
                                    + "'" + prop.FulizaBorrowed + "',"
                                    + "'" + prop.Charges + "',"
                                    + "'" + prop.FulizaAmount + "'"

                                    ;

                    SQLDataAccess.SaveData(szSQL);

                    

                }
                catch (Exception ex)
                {
                    Exception exe = ex;

                    throw exe;
                }
            }
            _CopyAndSaveCollectionsToRecepientsAndTransactions();
        }

        private static void _CopyAndSaveCollectionsToRecepientsAndTransactions()
        {
            string szSQL;

            szSQL = "EXECUTE RecepientsCopier";

            SQLDataAccess.SaveData(szSQL);

            szSQL = "EXECUTE TransactionsCopier";

            SQLDataAccess.SaveData(szSQL);
        }

        public static List<DL_XMLDataModel> LoadTransactionStatistics()
        {
                                                                                            string szSQL = string.Empty;

            szSQL = "SELECT "

                    + " [Code]                  AS Code              "
                    + ",[Code_ID]               AS Code_ID           "
                    + ",[M_Date]                AS Date              "
                    + ",[M_RecepientPhoneNo]    AS RecepientPhoneNo  "
                    + ",[M_CashAmount]          AS CashAmount        "
                    + ",[M_Balance]             AS Balance           "
                    + ",[M_PayBill_TillNo]      AS PayBill_TillNo    "
                    + ",[M_TransactionCost]     AS TransactionCost   "
                    + ",[M_Quota]               AS Quota             "
                    + ",[M_FulizaLimit]         AS FulizaLimit       "
                    + ",[M_FulizaBorrowed]      AS FulizaBorrowed    "
                    + ",[M_FulizaCharge]        AS FulizaCharge      "
                    + ",[M_FulizaAmount]        AS FulizaAmount      "

                    + "FROM[DB_MSota].[dbo].[Ms_Transactions]        "

                    ;

            return SQLDataAccess.LoadData<DL_XMLDataModel>(szSQL);
        }

        public static List<DL_XMLDataModel> LoadRecepientsStatistics()
        {
            string szSQL = string.Empty;

            szSQL = "SELECT "

                    + " [M_RecepientName]       AS RecepientName           "
                    + ",[M_RecepientAccNo]      AS RecepientAccNo              "
                    + ",[M_RecepientPhoneNo]    AS RecepientPhoneNo  "

                    + "FROM[DB_MSota].[dbo].[Ms_Recepients]        "

                    ;

            return SQLDataAccess.LoadData<DL_XMLDataModel>(szSQL);
        }
    }
}
