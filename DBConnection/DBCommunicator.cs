using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DBConnection
{
    class DBCommunicator
    {
        SqlConnection con = null;
        SqlCommand comm = null;
        SqlDataAdapter cmd = null;
        public void BeginCommunicator(string StrQuery)
        {
            using (con = new SqlConnection(@"Data Source=DESKTOP-53D0IES\MSTEST;Initial Catalog=MSota;User ID=sa;Password=manager;Encrypt=False"))
            using (var cmd = new SqlDataAdapter())
            using (SqlCommand comm = new SqlCommand(StrQuery))
            {
                comm.Connection = con;
                cmd.InsertCommand = comm;

                con.Open();
                comm.ExecuteNonQuery();
            }
        }
    }
}
