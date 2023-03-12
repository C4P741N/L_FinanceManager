using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace MSota.DataLibrary
{
    public class DbAccessor
    {
        public static List<DL_XMLDataModel> LoadTransactionStatistics => LoadData<DL_XMLDataModel>("SELECT "

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
        public static List<T> LoadData<T>(string sql)
        {
            using (IDbConnection IDbCn = new SqlConnection(GetConnectionString()))
            {
                return IDbCn.Query<T>(sql).ToList();
            }
        }

        public static string GetConnectionString()
        {

            string szConn = string.Empty;
            IConfiguration IConf = null;

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false);
            IConf = builder.Build();
            szConn = IConf.GetValue<string>("ConnectionStrings:value"); //Connection string from .json

            return szConn;
        }
    }
}
