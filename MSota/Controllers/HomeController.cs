using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSota.Accounts;

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

        public HomeController(ILogger<HomeController> logger, ITransactions transactions)
        {
            _logger = logger;
            _transactions = transactions;
        }

        [HttpPost]
        [Route("/[controller]/[action]/PostXmlData")]
        public HttpResponseMessage PostData()
        {
            //BeginDataInsertIf();

            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        [HttpGet]
        [Route("/[controller]/[action]/GetTransactionData")]
        public ActionResult GetTransactionStatistics()
        {
            MSota.Responses.TransactionsResponse trRp = _transactions.GetAllTransactions();
            if (trRp.Success)
            {
                return  Ok(trRp);
            }
            return BadRequest(trRp);
        }
    }
}