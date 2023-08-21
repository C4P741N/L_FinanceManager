using MSota.BaseFormaters;
using MSota.Controllers;
using MSota.DataServer;
using MSota.Extractors;
using MSota.Responses;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace MSota.JavaScriptObjectNotation
{
    public class JsonExtractor : IJsonExtractor
    {
        //JsonProps _jsonProps;
        IFortmaterAtLarge _fortmater;
        ISqlDataServer _sqlDataServer;
        IEx_SMS _ex_SMS;
        public JsonExtractor(IFortmaterAtLarge fortmater, ISqlDataServer sqlDataServer, IEx_SMS ex_SMS)
        {
            _fortmater = fortmater;
            _sqlDataServer = sqlDataServer;
            _ex_SMS = ex_SMS;
        }

        public BaseResponse ExtractBegin(string smsStringValue)
        {
            try
            {
                List<SMSMessages> smsMessages = xxMoveValuesToObject(smsStringValue);

                _sqlDataServer.PostData(smsMessages);

                return new BaseResponse(new Error(), HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return new BaseResponse(new Error(), HttpStatusCode.InternalServerError);
            }
        }

        private List<SMSMessages> xxMoveValuesToObject(string smsStringValue)
        {
            List<SMSMessages> smsMessages = new List<SMSMessages>();

            SMSMessages sMS = new SMSMessages();
            Values val = new Values();

            JObject json = JObject.Parse(smsStringValue);

            foreach (var item in json)
            {
                sMS.Key = item.Key;

                foreach (var it in item.Value)
                {
                    //if (item.Key == "MPESA")
                    //    _xfts.BeginExtractKcbData(lsMessage);

                    val.message = (string)it.SelectToken("message");
                    val.sender = (string)it.SelectToken("sender");

                    if (it.SelectToken("date") != null && it.SelectToken("date").Type == JTokenType.Integer)
                        val.date = _fortmater.DateConvertionFromLong((long)it.SelectToken("date"));
                    else
                        val.date = DateTime.MinValue; // Set a default value or handle the null case differently

                    val.read = (string)it.SelectToken("read");

                    if (it.SelectToken("type") != null && it.SelectToken("type").Type == JTokenType.Integer)
                        val.type = (int)it.SelectToken("type");
                    else
                        val.type = 0; // Set a default value or handle the null case differently

                    if (it.SelectToken("thread") != null && it.SelectToken("thread").Type == JTokenType.Integer)
                        val.thread = (int)it.SelectToken("thread");
                    else
                        val.thread = 0; // Set a default value or handle the null case differently

                    val.service = (string)it.SelectToken("service");

                    if (string.IsNullOrEmpty(val.message) == false)
                        val.smsProps = _ex_SMS.MessageExtractBegin(sMS.Key,val);

                    sMS.Values.Add(val);
                }

                smsMessages.Add(sMS);
            }


            return smsMessages;

        }
    }
}
