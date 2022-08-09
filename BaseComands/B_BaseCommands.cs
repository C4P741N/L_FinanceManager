using ExtensibleMarkupAtLarge;
using DBConnection;
using DataAndStatistics;

namespace BaseCommands
{
    public class B_BaseCommands
    {
        private X_ExtensibleMarkupAtLarge xml_al = null;
        private DBRecordSet rs = null;
        private VariablesFormater vf = null;
        public void BeginLaunchOfXMLStuff()
        {
            xx_GetXMLValues();
        }

        private void xx_GetXMLValues()
        {
            xml_al = new X_ExtensibleMarkupAtLarge();
            rs = new DBRecordSet();
            List<X_XMLProperties> x_prop = new List<X_XMLProperties>();

            xml_al.BeginGetXMLValues(ref x_prop);
            rs.BeginDBConnectionAndDataInsert(x_prop);

        }

        public void BeginLaunchOfStuffToGetData(ref U_StatisticsProp uisprop)
        {
            xx_GetDataAndStuff(ref uisprop);
        }

        private void xx_GetDataAndStuff(ref U_StatisticsProp uisprop)
        {
            vf = new VariablesFormater();

            vf.BeginFormatingVariables(ref uisprop);

        }
    }
}