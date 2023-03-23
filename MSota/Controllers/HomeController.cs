using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSota.Accounts;
using MSota.ExtensibleMarkupAtLarge;
using MSota.Responses;

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

        public HomeController(ILogger<HomeController> logger, ITransactions transactions, IXmlExtractor XmlExtractor)
        {
            _logger = logger;
            _transactions = transactions;
            _XmlExtractor = XmlExtractor;
        }

        [HttpPost]
        [Route("/[controller]/[action]/PostXmlData")]
        public HttpResponseMessage PostData()
        {
            BaseResponse baseResponse = _XmlExtractor.DBUpdateFromXmlFile();
            if (baseResponse._success)
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.Created
                };
            }

            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotModified
            };
        }

        [HttpPost]
        [Route("/[controller]/[action]/GetTransactionData")]
        public ActionResult GetTransactionStatistics([FromBody]Date date)
        {
            var test0 = date;

            MSota.Responses.TransactionsResponse trRp = _transactions.GetAllTransactions();
            if (trRp._success)
            {
                return  Ok(trRp);
            }
            return BadRequest(trRp);
        }
    }

    public class Date
    {
        public DateTime to { get; set; }
        public DateTime from { get; set; }
        public DateTime defaultmax { get; set; }
    }
}