using System.Xml;
//using StringFormaterAtLarge;

namespace ExtensibleMarkupAtLarge
{
    public class X_ExtensibleMarkupAtLarge
    {
		private System.Xml.XmlDocument xml_doc = null;
		private System.IO.StreamReader sr = null;
		private XmlAttributeCollection xmlatt = null;
        private X_XMLProperties xml_prop = null;
        private X_ExtensibleMarkupStructure xml_stru = null;
        private X_XMLFormartToString xfts = null;

        private string szDocPath = @"D:\_notTemp\Proj\MSota\sms-20220711200832.xml";

		public void BeginGetXMLValues()
        {
			InitialitzeStuff();
            BeginRetrivingXMLFileAndGettingValues();
        }

        private void BeginRetrivingXMLFileAndGettingValues()
        {
			List<X_XMLProperties> lsMessages = MessageListFromXML();
			List<X_XMLProperties> x_prop = new List<X_XMLProperties>();


			foreach (X_XMLProperties lsMessage in lsMessages)
            {

                if (lsMessage.szAddress =="MPESA")
                {
					//X_XMLProperties messageUpdate = _ExtractBodyForMPESA(lsMessage);
					xfts.BeginMPESAFormatToString(lsMessage);
					x_prop.Add(lsMessage);
				}
				if (lsMessage.szAddress == "KCB")
				{
                    xfts.BeginKCBFormatToString(lsMessage);
                    //x_prop.Add(lsMessage);
                }


			}
		}
		private void InitialitzeStuff()
        {
			xml_doc = new System.Xml.XmlDocument();
			sr = new System.IO.StreamReader(szDocPath);
			xml_stru = new X_ExtensibleMarkupStructure();
            xfts = new X_XMLFormartToString();
			//xml_prop = new X_XMLProperties();

		}
		private List<X_XMLProperties>  MessageListFromXML()
		{
																						string szXmlString = string.Empty;
																						string szName = string.Empty;
																						string szAttName = string.Empty;
																						string szValue = string.Empty;
																						string szParentNodeName = string.Empty;

			szXmlString = sr.ReadToEnd();

			xml_doc.LoadXml(szXmlString);

			XmlNodeList XMLParentNodes = xml_doc.GetElementsByTagName("smses");

			List<X_XMLProperties> ls = new List<X_XMLProperties>();

			foreach (XmlNode node in XMLParentNodes)
			{
				//node.Attributes.
				szParentNodeName = node.Name;

				XmlNodeList XMLChildNodes = node.ChildNodes;

				foreach (XmlNode childnode in XMLChildNodes)
				{
					xmlatt = childnode.Attributes;
					xml_prop = new X_XMLProperties();

					if (childnode.Attributes["address"].Value is "MPESA" or "KCB")
					{
						xml_prop.szProtocol			= xmlatt["protocol"].Value;
						xml_prop.szAddress			= xmlatt["address"].Value;
						xml_prop.szDate				= xmlatt["date"].Value;
						xml_prop.szType				= xmlatt["type"].Value;
						xml_prop.szSubject			= xmlatt["subject"].Value;
						xml_prop.szBody				= xmlatt["body"].Value;
						xml_prop.szToa				= xmlatt["toa"].Value;
						xml_prop.szSc_toa			= xmlatt["sc_toa"].Value;
						xml_prop.szService_center	= xmlatt["service_center"].Value;
						xml_prop.szRead				= xmlatt["read"].Value;
						xml_prop.szStatus			= xmlatt["status"].Value;
						xml_prop.szLocked			= xmlatt["locked"].Value;
						xml_prop.szDate_sent		= xmlatt["date_sent"].Value;
						xml_prop.szSub_id			= xmlatt["sub_id"].Value;
						xml_prop.szReadable_date	= xmlatt["readable_date"].Value;
						xml_prop.szContact_name		= xmlatt["contact_name"].Value;

                        ls.Add(xml_prop);
					}
				}
			}

			//xml_stru.BeginXMLStructureBuild(ls);
			return ls;
        }
	}
}