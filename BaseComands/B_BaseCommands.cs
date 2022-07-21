using ExtensibleMarkupAtLarge;
using DBConnection;

namespace BaseCommands
{
    public class B_BaseCommands
    {
        private X_ExtensibleMarkupAtLarge xml_al = null;
        private RecordSet rs = null;
        private RecordGet rg = null;
        public void BeginLaunchOfXMLStuff()
        {
            xx_GetXMLValues();
        }

        private void xx_GetXMLValues()
        {
            xml_al = new X_ExtensibleMarkupAtLarge();
            rs = new RecordSet();
            List<X_XMLProperties> x_prop = new List<X_XMLProperties>();

            xml_al.BeginGetXMLValues(ref x_prop);
            rs.BeginDBConnectionAndDataInsert(x_prop);

        }

        public void BeginLaunchOfStuffToGetData()
        {
            xx_GetDataAndStuff();
        }

        private void xx_GetDataAndStuff()
        {
            rg = new RecordGet();

            rg.BeginGetDataAndStuff();

        }
    }
}