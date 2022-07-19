using ExtensibleMarkupAtLarge;
using DBConnection;

namespace BaseCommands
{
    public class B_BaseCommands
    {
        private X_ExtensibleMarkupAtLarge xml_al = null;
        private DatabaseConnection db_con = null;
        public void BeginLaunchOfStuff()
        {
            xx_GetXMLValues();
        }

        private void xx_GetXMLValues()
        {
            xml_al = new X_ExtensibleMarkupAtLarge();
            db_con = new DatabaseConnection();
            List<X_XMLProperties> x_prop = new List<X_XMLProperties>();

            xml_al.BeginGetXMLValues(ref x_prop);
            db_con.BeginDBConnection(x_prop);

        }
    }
}