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
            //BeginDataInsertIf();
            _XmlExtractor.DBUpdateFromXmlFile();
            if (true)
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

        //public BaseResponse test()
        //{
        //    MSota.Responses.BaseResponse response = null;

        //    return response;
        //}

        [HttpGet]
        [Route("/[controller]/[action]/GetTransactionData")]
        public ActionResult GetTransactionStatistics()
        {
            MSota.Responses.TransactionsResponse trRp = _transactions.GetAllTransactions();
            if (trRp._success)
            {
                return  Ok(trRp);
            }
            return BadRequest(trRp);
        }
    }
}