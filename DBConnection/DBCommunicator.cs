using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DBConnection
{
    class DBCommunicator
    {
        SqlConnection oCon = null;
        SqlCommand oCmd = null;
        SqlDataAdapter oAdap = null;
        //string strConAd = @"Data Source=DESKTOP-19HJDEM;Initial Catalog=MSota;Integrated Security=True;Encrypt=False";
        string strConAd = @"Data Source=DESKTOP-53D0IES\MSTEST;Initial Catalog=MSota;User ID=sa;Password=manager;Encrypt=False";
        public void BeginCommunicator(string strvQuery)
        {
            using (oCon = new SqlConnection(strConAd))
            using (var oAdap = new SqlDataAdapter())
            using (SqlCommand oCmd = new SqlCommand(strvQuery, oCon))
            {
                oCmd.Connection = oCon;

                oAdap.InsertCommand = oCmd;

                oCon.Open();
                oCmd.ExecuteNonQuery();
            }
        }
        public void BeginCommunicatorRetrive(string strvQuery, ref SqlDataReader oReader)
        {
            oCon = new SqlConnection(strConAd);
            //using (con = new SqlConnection(@"Data Source=DESKTOP-53D0IES\MSTEST;Initial Catalog=MSota;User ID=sa;Password=manager;Encrypt=False"))
            var oAdap = new SqlDataAdapter();
            SqlCommand oCmd = new SqlCommand(strvQuery, oCon);
            
            oCmd.Connection = oCon;

            oAdap.SelectCommand = oCmd;

            oCon.Open();

            oReader = oCmd.ExecuteReader();

        }
        public void BeginCommunicatorClose()
        {
            oCon = new SqlConnection(strConAd);
            oCon.Close();

        }
    }
}
