using ExtensibleMarkupAtLarge;

namespace BaseCommands
{
    public class B_BaseCommands
    {
        private X_ExtensibleMarkupAtLarge xml_al = null;
        public void BeginLaunchOfStuff()
        {
            GetXMLValues();
        }

        private void GetXMLValues()
        {
            xml_al = new X_ExtensibleMarkupAtLarge();

            xml_al.BeginGetXMLValues();

        }
    }
}