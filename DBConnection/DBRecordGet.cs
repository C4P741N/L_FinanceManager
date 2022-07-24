using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnection
{
    public class DBRecordGet
    {
        private DBCommunicator dbc = null;
        private SqlDataReader oReader = null;
        public void BeginGetDataAndStuff(ref SqlDataReader oReader)
        {
            dbc = new DBCommunicator();
            //oReader = new SqlDataReader();

            xx_StartGettingDataAndStuff(ref oReader);
        }

        private void xx_StartGettingDataAndStuff(ref SqlDataReader oReader)
        {
            string strQuery = string.Empty;

            strQuery = "SELECT "

                    + " [Code]                  AS Code                "
                    + ",[M_Date]                AS M_Date              "
                    + ",[M_RecepientName]       AS M_RecepientName     "
                    + ",[M_RecepientPhoneNo]    AS M_RecepientPhoneNo  "
                    + ",[M_RecepientDate]       AS M_RecepientDate     "
                    + ",[M_RecepientAccNo]      AS M_RecepientAccNo    "
                    + ",[M_CashAmount]          AS M_CashAmount        "
                    + ",[M_Balance]             AS M_Balance           "
                    + ",[M_szProtocol]          AS M_szProtocol        "
                    + ",[M_TransactionStatus]   AS M_TransactionStatus "
                    + ",[M_TransactionCost]     AS M_TransactionCost   "
                    + ",[M_Address]             AS M_Address           "
                    + ",[M_Type]                AS M_Type              "
                    + ",[M_Subject]             AS M_Subject           "
                    + ",[M_Body]                AS M_Body              "
                    + ",[M_Toa]                 AS M_Toa               "
                    + ",[M_Sc_toa]              AS M_Sc_toa            "
                    + ",[M_Service_center]      AS M_Service_center    "
                    + ",[M_Read]                AS M_Read              "
                    + ",[M_Locked]              AS M_Locked            "
                    + ",[M_Date_sent]           AS M_Date_sent         "
                    + ",[M_Status]              AS M_Status            "
                    + ",[M_Sub_id]              AS M_Sub_id            "
                    + ",[M_Readable_date]       AS M_Readable_date     "
                    + ",[M_szContact_name]      AS M_szContact_name    "
                    + ",[M_Quota]               AS M_Quota             "
                    + ",[M_FulizaLimit]         AS M_FulizaLimit       "
                    + ",[M_FulizaBorrowed]      AS M_FulizaBorrowed    "
                    + ",[M_FulizaCharge]        AS M_FulizaCharge      "
                    + ",[M_FulizaAmount]        AS M_FulizaAmount      "

                    + "FROM[MSota].[dbo].[Ms_Collection]";

            

            dbc.BeginCommunicatorRetrive(strQuery, ref oReader);
            dbc.BeginCommunicatorClose();

            //if (oReader.HasRows)
            //{
            //    while (oReader.Read())
            //    {
            //        //DataAndStatistics.DataAndStatisticsProp.
            //        string rop = oReader["Code"].ToString();

            //        Debug.WriteLine("{0} {1} {2} {3} {4}"
            //                        , oReader["Code"], oReader["M_Date"], oReader["M_RecepientName"], oReader["M_RecepientPhoneNo"], oReader["Code"]);
            //    }
            //}


        }
        //public Person SomeMethod(string fName)
        //{
        //    var con = ConfigurationManager.ConnectionStrings["Yourconnection"].ToString();

        //    Person matchingPerson = new Person();
        //    using (SqlConnection myConnection = new SqlConnection(con))
        //    {
        //        string oString = "Select * from Employees where FirstName=@fName";
        //        SqlCommand oCmd = new SqlCommand(oString, myConnection);
        //        oCmd.Parameters.AddWithValue("@Fname", fName);
        //        myConnection.Open();
        //        using (SqlDataReader oReader = oCmd.ExecuteReader())
        //        {
        //            while (oReader.Read())
        //            {
        //                matchingPerson.firstName = oReader["FirstName"].ToString();
        //                matchingPerson.lastName = oReader["LastName"].ToString();
        //            }

        //            myConnection.Close();
        //        }
        //    }
        //    return matchingPerson;
        //}
    }
}
