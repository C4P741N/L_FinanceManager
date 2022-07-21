using ExtensibleMarkupAtLarge;
using System.Web.Http;
//using Microsoft.Data.SqlClient;

namespace DBConnection
{
    public class RecordSet
    {
        private DBCommunicator dbc = null;

        List<X_XMLProperties> x_prop = null;
        //SqlConnection con = null;
        //SqlCommand comm = null;
        //SqlDataAdapter cmd = null;
        HttpResponseMessage response = null;
        HttpResponseException exception = null;

        public void BeginDBConnectionAndDataInsert(List<X_XMLProperties> x_vprop)
        {
            x_prop = new List<X_XMLProperties>();
            response = new HttpResponseMessage();
            exception = new HttpResponseException(response);
            dbc = new DBCommunicator();

            x_prop = x_vprop;

            xx_StartConnectionAndDataInsert();
        }
        private void xx_StartConnectionAndDataInsert()
        {
                                                                                        string StrQuery = string.Empty;
                                                                                        bool conFailed = false;

            while (true)
            {
                foreach (X_XMLProperties prop in x_prop)
                {
                    try
                    {
                        StrQuery = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                        + "' " + prop.Code + "' ,"
                                        + "' " + prop.szDate + "' ,"
                                        + "' " + prop.RName + "' ,"
                                        + "' " + prop.RPhoneNo + "' ,"
                                        + "' " + prop.RDate + "' ,"
                                        + "' " + prop.RAccNo + "' ,"
                                        + "' " + prop.CashAmount + "' ,"
                                        + "' " + prop.Balance + "' ,"
                                        + "' " + prop.szProtocol + "' ,"
                                        + "' " + prop.TransactionStatus + "' ,"
                                        + "' " + prop.TransactionCost + "' ,"
                                        + "' " + prop.szAddress + "' ,"
                                        + "' " + prop.szType + "' ,"
                                        + "' " + prop.szSubject + "' ,"
                                        + "' " + prop.szBody + "' ,"
                                        + "' " + prop.szToa + "' ,"
                                        + "' " + prop.szSc_toa + "' ,"
                                        + "' " + prop.szService_center + "' ,"
                                        + "' " + prop.szRead + "' ,"
                                        + "' " + prop.szLocked + "' ,"
                                        + "' " + prop.szDate_sent + "' ,"
                                        + "' " + prop.szStatus + "' ,"
                                        + "' " + prop.szSub_id + "' ,"
                                        + "' " + prop.RAccNo + "' ,"
                                        + "' " + prop.szContact_name + "' ,"
                                        + "' " + prop.Quota + "' ,"
                                        + "' " + prop.FulizaLimit + "' ,"
                                        + "' " + prop.FulizaBorrowed + "' ,"
                                        + "' " + prop.FulizaCharge + "' ,"
                                        + "' " + prop.FulizaAmount + "'  "

                                        ;

                        dbc.BeginCommunicator(StrQuery);
                        //using (con = new SqlConnection(@"Data Source=DESKTOP-53D0IES\MSTEST;Initial Catalog=MSota;User ID=sa;Password=manager;Encrypt=False"))
                        //using (var cmd = new SqlDataAdapter())
                        //using (SqlCommand comm = new SqlCommand(StrQuery))
                        //{
                        //    comm.Connection = con;
                        //    cmd.InsertCommand = comm; 

                        //    con.Open();
                        //    comm.ExecuteNonQuery();
                        //}
                    }
                    catch (Exception ex)
                    {
                        Exception exe = exception;

                        throw exe;
                    }
                }
                break;
            }
        }
    }
}