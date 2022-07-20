using ExtensibleMarkupAtLarge;
using Microsoft.Data.SqlClient;
using System.Web.Http;

namespace DBConnection
{
    public class DatabaseConnection
    {
        List<X_XMLProperties> x_prop = null;
        SqlConnection con = null;
        SqlCommand comm = null;
        SqlDataAdapter cmd = null;
        HttpResponseMessage response = null;
        HttpResponseException exception = null;
        public void BeginDBConnection(List<X_XMLProperties> x_vprop)
        {
            x_prop = new List<X_XMLProperties>();
            response = new HttpResponseMessage();
            exception = new HttpResponseException(response);

            x_prop = x_vprop;

            xx_StartConnection();
        }
        private void xx_StartConnection()
        {
                                                                                        string StrQuery = string.Empty;
                                                                                        bool conFailed = false;

            while (!conFailed)
            {
                foreach (X_XMLProperties prop in x_prop)
                {
                    try
                    {
                            StrQuery = "INSERT INTO Ms_Collection VALUES"
                                        + "( "
                                        + "' " + prop.Code + "' ,"
                                        + "' " + prop.Balance + "' ,"
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
                                        + ") ";


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
                    catch (Exception ex)
                    {
                        Exception exe = exception;
                        conFailed = true;

                        throw exe;
                    }
                }
                break;
            }
        }
    }
}