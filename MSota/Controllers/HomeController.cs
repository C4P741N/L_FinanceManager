using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSota.Accounts;
using MSota.ExtensibleMarkupAtLarge;
using MSota.JavaScriptObjectNotation;
using MSota.Models;
using MSota.Responses;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;

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
        private IJsonExtractor _jsonExtractor;

        public HomeController(ILogger<HomeController> logger, ITransactions transactions, IXmlExtractor XmlExtractor, IFactions factions, IJsonExtractor jsonExtractor)
        {
            _logger = logger;
            _transactions = transactions;
            _XmlExtractor = XmlExtractor;
            _factions = factions;
            _jsonExtractor = jsonExtractor;
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
        [Route("/[controller]/[action]/postJson")]
        public ActionResult PostJsonData([FromBody] string smsMessage)
        {
            if (smsMessage == null )
            {
                return UnprocessableEntity("Invalid data");
            }

            BaseResponse baseResponse = _jsonExtractor.UpdateFromJson(smsMessage);

            if (!baseResponse._error.bErrorFound)
            {
                return Ok(baseResponse);
            }

            return BadRequest(baseResponse);
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