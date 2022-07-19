using ExtensibleMarkupAtLarge;

namespace DBConnection
{
    public class DatabaseConnection
    {
        //private X_XMLProperties xml_prop = null;
        public void BeginDBConnection(List<X_XMLProperties> x_vprop)
        {
            //xml_prop = new ExtensibleMarkupAtLarge.X_XMLProperties();

            xx_StartConnection(x_vprop);
        }
        private void xx_StartConnection(List<X_XMLProperties> x_vprop)
        {
            List<X_XMLProperties> x_prop = new List<X_XMLProperties>();
            x_prop = x_vprop;
        }
    }
}