using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSota.Accounts;
using MSota.ExtensibleMarkupAtLarge;
using MSota.Models;
using MSota.Responses;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSota.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private ITransactions _transactions;
        private IXmlExtractor _XmlExtractor;
        private IFactions _factions;

        public HomeController(ILogger<HomeController> logger, ITransactions transactions, IXmlExtractor XmlExtractor, IFactions factions)
        {
            _logger = logger;
            _transactions = transactions;
            _XmlExtractor = XmlExtractor;
            _factions = factions;
        }

        [HttpPost]
        [Route("/[controller]/[action]/PostXmlData")]
        public ActionResult PostData()
        {
            BaseResponse baseResponse = _XmlExtractor.DBUpdateFromXmlFile();
            if (!baseResponse._error.bErrorFound)
            {
                return Ok(baseResponse);
            }

            return BadRequest(baseResponse);
        }

        [HttpPost]
        [Route("/[controller]/[action]/postJsonTest")]
        public ActionResult PostJsonDataTest([FromBody] string jsonData)
        {
            try
            {
                // Parse the JSON data into a JObject
                JObject jsonObject = JObject.Parse(jsonData);

                // If you're using Newtonsoft.Json, you can deserialize the JSON data into your class
                BaseResponse baseResponse = jsonObject.ToObject<BaseResponse>();

                // Call the method with the deserialized object
                //baseResponse = _XmlExtractor.DBUpdateFromXmlFile(baseResponse);

                if (!baseResponse._error.bErrorFound)
                {
                    return Ok(baseResponse);
                }

                return BadRequest(baseResponse);
            }
            catch (JsonException)
            {
                // If the JSON data is invalid and cannot be deserialized to BaseResponse, return BadRequest
                return BadRequest("Invalid JSON format.");
            }
        }

        [HttpPost]
        [Route("/[controller]/[action]/postJson")]
        public ActionResult PostJsonData([FromBody] CustomArrayList jsonData)
        {
            if (jsonData == null)
            {
                return BadRequest("Invalid JSON data");
            }
            return Ok("JSON data received and processed successfully.");
        }


        [HttpPost]
        [Route("/[controller]/[action]/GetTransactionData")]
        public ActionResult GetTransactionStatistics([FromBody]Calendar cal)
        {
            MSota.Responses.TransactionsResponse trRp = _transactions.GetAllTransactions(cal);
            if (!trRp._error.bErrorFound)
            {
                return  Ok(trRp);
            }
            return BadRequest(trRp);
        }

        [HttpPost]
        [Route("/[controller]/[action]/GetFactionsList")]
        public ActionResult FactionsList([FromBody] string factionId)
        {
            MSota.Responses.FactionsResponse frRp = _factions.GetFactionList(factionId);
            if (!frRp._error.bErrorFound)
            {
                return Ok(frRp);
            }
            return BadRequest(frRp);
        }
    }

    public class SMSMessage
    {
        public string ?message { get; set; }
        public string ?sender { get; set; }
        public long date { get; set; }
        public bool read { get; set; }
        public int type { get; set; }
        public int thread { get; set; }
        public string ?service { get; set; }
    }

    public class SMSMessageData
    {
        // Create a dictionary with string as the key and a list of SMSMessage as the value
        public string ?key { get; set; }
        //public Dictionary<string, List<SMSMessage>> ?value { get; set; }

        public List<SMSMessage> value { get; set; } = new List<SMSMessage>();
    }

    public class SMSMessagesBody
    {
        //public Dictionary<string, List<SMSMessageData>> ?SMSMessageData { get; set; }

        public List<SMSMessageData> smsMessage { get; set; } = new List<SMSMessageData>();
    }

    public class CustomArrayList
    {
        public List<object> smsMessage { get; set; }
    }
}