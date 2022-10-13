using ExtensibleMarkupAtLarge;
using Microsoft.Data.SqlClient;
using System.Web.Http;
//using Microsoft.Data.SqlClient;

namespace DBConnection
{
    public class DBRecordSet
    {
        private DBCommunicator dbc = null;

        List<X_XMLProperties> x_prop = null;
        HttpResponseMessage response = null;
        HttpResponseException exception = null;
        private SqlDataReader oReader = null;

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
                                                                                        string strQuery = string.Empty;
                                                                                        bool conFailed = false;

            while (true)
            {
                foreach (X_XMLProperties prop in x_prop)
                {
                    try
                    {
                        strQuery = "EXECUTE Ms_DuplicateChecker " //This part gives me joy

                                        + "'" + prop.Code + "',"
                                        + "'" + prop.szDate + "',"
                                        + "'" + prop.RName + "',"
                                        + "'" + prop.RPhoneNo + "',"
                                        + "'" + prop.RDate + "',"
                                        + "'" + prop.RAccNo + "',"
                                        + "'" + prop.CashAmount + "',"
                                        + "'" + prop.Balance + "',"
                                        + "'" + prop.szProtocol + "',"
                                        + "'" + prop.TransactionStatus + "',"
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
                                        + "'" + prop.szContact_name + "',"
                                        + "'" + prop.Quota + "',"
                                        + "'" + prop.FulizaLimit + "',"
                                        + "'" + prop.FulizaBorrowed + "',"
                                        + "'" + prop.FulizaCharge + "',"
                                        + "'" + prop.FulizaAmount + "'"

                                        ;

                        dbc.BeginCommunicator(strQuery);
                       
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