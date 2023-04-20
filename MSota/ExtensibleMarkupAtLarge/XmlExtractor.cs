using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using MSota.BaseFormaters;
using MSota.DataLibrary;
using MSota.Responses;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MSota.ExtensibleMarkupAtLarge
{
    public class XmlExtractor : IXmlExtractor
    {
        //IXmlProps _xmlProps;
        IXmlDataFotmater _xfts;
        IFortmaterAtLarge _formatter;
        ISqlDataServer _sqlDataServer;
        System.Xml.XmlDocument xml_doc = new System.Xml.XmlDocument();
        //Error _error;
        public XmlExtractor(IXmlDataFotmater xfts, IFortmaterAtLarge formatter, ISqlDataServer sqlDataServer)
        {
            //_xmlProps = xmlProps;
            _xfts = xfts;
            _formatter = formatter;
            _sqlDataServer = sqlDataServer;
            //_error = error;
        }

        public BaseResponse DBUpdateFromXmlFile()
        {
            try
            {
                List<IXmlProps> lsMessages = xx_MessageListFromXML();
                List<IXmlProps> x_prop = new List<IXmlProps>();

                //string[] SzCollName = Array.Empty<string>();
                //string[] SzCollPhonNo = new string[lsMessages.Count];

                var SzCollName = new Dictionary<string, string>();

                int count = 0;

                foreach (IXmlProps lsMessage in lsMessages)
                {

                    if (lsMessage.szAddress == "MPESA")
                        _xfts.BeginExtractMpesaData(lsMessage);

                    if (lsMessage.szAddress == "KCB")
                        _xfts.BeginExtractKcbData(lsMessage);

                    if (lsMessage.szQuota != EnumsAtLarge.EnumContainer.TransactionQuota.None)
                    {
                        if (lsMessage.szRName != "Fuliza")
                        {
                            if (SzCollName.ContainsKey(lsMessage.szRName + lsMessage.szRAccNo) is false)
                            {
                                lsMessage.szUniqueKey = _formatter.GetUniqueKey();

                                SzCollName.Add(lsMessage.szRName + lsMessage.szRAccNo, lsMessage.szUniqueKey);
                            }
                            else
                            {
                                string Code = SzCollName.GetValueOrDefault(lsMessage.szRName + lsMessage.szRAccNo);

                                lsMessage.szUniqueKey = Code;
                            }
                        }
                        x_prop.Add(lsMessage);
                    }
                    count++;
                }
                SzCollName.Clear();
                GC.Collect();

                _sqlDataServer.PostData(x_prop);

                return new BaseResponse(new Error(),HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {

                using var reader = new StringReader(ex?.StackTrace);
                string? firstStackTraceLine = reader.ReadLine();

                return new BaseResponse(new Error
                {
                    szErrorMessage = ex.Message,
                    szStackTrace = firstStackTraceLine,
                    bErrorFound = true,
                }, HttpStatusCode.InternalServerError);
            }
        }
        private List<IXmlProps> xx_MessageListFromXML()
        {
            string szXmlString = string.Empty;
            string szName = string.Empty;
            string szAttName = string.Empty;
            string szValue = string.Empty;
            string szParentNodeName = string.Empty;

            var xmlFiles = Directory.GetFiles($"{AppDomain.CurrentDomain.BaseDirectory}doc\\", "*.xml");

            List<IXmlProps> ls = new List<IXmlProps>();

            for (int i = 0; i < xmlFiles.Count(); i++)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(xmlFiles[i]);

                szXmlString = sr.ReadToEnd();

                xml_doc.LoadXml(szXmlString);

                XmlNodeList XMLParentNodes = xml_doc.GetElementsByTagName("smses");

                foreach (XmlNode node in XMLParentNodes)
                {
                    szParentNodeName = node.Name;

                    XmlNodeList XMLChildNodes = node.ChildNodes;

                    foreach (XmlNode childnode in XMLChildNodes)
                    {
                        XmlAttributeCollection xmlatt = childnode.Attributes;
                         var xmlProps = new XmlProps();

                        if (childnode.Attributes["address"].Value is "MPESA" or "KCB")
                        {
                            xmlProps.szProtocol = xmlatt["protocol"].Value;
                            xmlProps.szAddress = xmlatt["address"].Value;
                            xmlProps.szDate = xmlatt["date"].Value;
                            xmlProps.szType = xmlatt["type"].Value;
                            xmlProps.szSubject = xmlatt["subject"].Value;
                            xmlProps.szBody = xmlatt["body"].Value;
                            xmlProps.szToa = xmlatt["toa"].Value;
                            xmlProps.szSc_toa = xmlatt["sc_toa"].Value;
                            xmlProps.szService_center = xmlatt["service_center"].Value;
                            xmlProps.szRead = xmlatt["read"].Value;
                            xmlProps.szStatus = xmlatt["status"].Value;
                            xmlProps.szLocked = xmlatt["locked"].Value;
                            xmlProps.szDate_sent = xmlatt["date_sent"].Value;
                            xmlProps.szSub_id = xmlatt["sub_id"].Value;
                            xmlProps.szReadable_date = xmlatt["readable_date"].Value;

                            ls.Add(xmlProps);
                        }
                    }
                }
            }
            return ls;
        }
    }
}
