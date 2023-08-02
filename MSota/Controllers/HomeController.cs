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
        [Route("/[controller]/[action]/PostJsonData")]
        public ActionResult PostJsonData([FromBody] string jsonData)
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

    
}