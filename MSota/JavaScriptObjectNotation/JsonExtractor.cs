﻿using Microsoft.AspNetCore.Components.Forms;
using MSota.BaseFormaters;
using MSota.Controllers;
using MSota.DataServer;
using MSota.Extractors;
using MSota.Models;
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

        public BaseResponse UpdateFromJson(string smsStringValue)
        {
            try
            {
                Task<HttpStatusCode> status = _BeginAndPostUpdate(smsStringValue);
                GC.Collect();

                return new BaseResponse(new Error(), status.Result);
            }
            catch (Exception ex)
            { 
                return new BaseResponse(new Error
                {
                    szErrorMessage = ex.Message,
                    bErrorFound = true,
                }, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<HttpStatusCode> _BeginAndPostUpdate(string smsStringValue)
        {
            JObject json = JObject.Parse(smsStringValue);

            foreach (KeyValuePair<string, JToken> item in json)
            {
                foreach (JToken token in item.Value)
                {
                    JsonBodyModel vals = new JsonBodyModel();
                    vals.DocType = item.Key;

                    vals = _ParseJson(vals, token);

                    await Task.Run(() => _sqlDataServer.PostData(vals)); // Wrapping the non-async method call
                }
            }

            return HttpStatusCode.Accepted;
        }

        private JsonBodyModel _ParseJson(JsonBodyModel vals, JToken token)
        {
            vals.Body = token.Value<string>("message");
            vals.sender = token.Value<string>("sender");

            long dateValue = token.SelectToken("date")?.Value<long>() ?? 0;
            vals.DocDateTime = dateValue > 0 ? _fortmater.DateConvertionFromLong(dateValue) : DateTime.MinValue;

            vals.IsRead = token.Value<bool>("read") ? 1 : 0;
            vals.LongDate = dateValue;
            vals.type = token.Value<int>("type");
            vals.thread = token.Value<int>("thread");
            vals.Service_center = token.Value<string>("service");

            if (!string.IsNullOrEmpty(vals.Body))
                vals.smsProps = _ex_SMS.MessageExtractBegin(vals);

            return vals;
        }


    }
}
