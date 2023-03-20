﻿using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using MSota.BaseFormaters;
using MSota.DataLibrary;
using MSota.Responses;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace MSota.ExtensibleMarkupAtLarge
{
    public class XmlExtractor : IXmlExtractor
    {
        IXmlProps _xmlProps;
        IXmlDataFotmater _xfts;
        IFortmaterAtLarge _formatter;
        ISqlDataServer _sqlDataServer;
        System.Xml.XmlDocument xml_doc = new System.Xml.XmlDocument();
        //Error _error;
        public XmlExtractor(IXmlProps xmlProps, IXmlDataFotmater xfts, IFortmaterAtLarge formatter, ISqlDataServer sqlDataServer)
        {
            _xmlProps = xmlProps;
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

                Collection col = new Collection();

                foreach (IXmlProps lsMessage in lsMessages)
                {

                    if (lsMessage.szAddress == "MPESA")
                        _xfts.BeginExtractMpesaData(lsMessage);

                    if (lsMessage.szAddress == "KCB")
                        _xfts.BeginExtractKcbData(lsMessage);

                    if (lsMessage.szQuota != null)
                    {
                        lsMessage.szRName = _formatter.StringFormaterToProperCase(lsMessage.szRName);

                        if (lsMessage.szRName != "Fuliza")
                        {
                            if (col.Contains(lsMessage.szRName) is false)
                            {
                                lsMessage.szUniqueKey = _formatter.GetUniqueKey();

                                col.Add("", lsMessage.szRName);
                                if (string.IsNullOrEmpty(lsMessage.szRPhoneNo) is false)
                                {
                                    col.Add("", lsMessage.szRPhoneNo);
                                }
                            }
                            else if (string.IsNullOrEmpty(lsMessage.szRPhoneNo) is false && col.Contains(lsMessage.szRPhoneNo) is false)
                            {
                                lsMessage.szUniqueKey = _formatter.GetUniqueKey();

                                col.Add("", lsMessage.szRPhoneNo);
                            }
                        }
                        x_prop.Add(lsMessage);
                    }

                }
                col.Clear();
                GC.Collect();

                _sqlDataServer.PostData(x_prop);

                return new BaseResponse(new Error());
            }
            catch (Exception ex)
            {
                return new BaseResponse(new Error
                {
                    szErrorMessage = ex.Message,
                    szStackTrace = ex.StackTrace,
                    bErrorFound = true
                });
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