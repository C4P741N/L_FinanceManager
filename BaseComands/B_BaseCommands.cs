using ExtensibleMarkupAtLarge;
using static DataLibrary.BusinessLogic.StatisticsProcessor;
//using DBConnection;
using DataAndStatistics;
using LogicObjects;
using System.Transactions;

namespace BaseCommands
{
    public class B_BaseCommands
    {
        private X_ExtensibleMarkupAtLarge xml_al = null;
        //private DBRecordSet rs = null;
        private VariablesFormater vf = null;
        public static void BeginDataInsertIf()
        {
            B_BaseCommands bcmd = new B_BaseCommands();

            bcmd.xx_GetXMLValues();
        }

        private void xx_GetXMLValues()
        {
            xml_al = new X_ExtensibleMarkupAtLarge();
            var x_prop = new List<X_XMLProperties>();

            xml_al.BeginGetXMLValues(ref x_prop);

            AddStatisticsToDb(x_prop);

        }

        public static L_Transactions Transactions()
        {
            VariablesFormater vf = new VariablesFormater();

            return vf.BeginGettingTransactions();
        }
    }
}