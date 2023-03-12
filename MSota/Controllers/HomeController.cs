using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using static MSota.Base.BaseCommands;
using MSota.Models;
using MSota.DataLibrary;

namespace MSota.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private ITransactionsResponse _transactionsResponse;

        public HomeController(ILogger<HomeController> logger, ITransactionsResponse transactionsResponse)
        {
            _logger = logger;
            _transactionsResponse = transactionsResponse;
        }

        [HttpPost]
        [Route("/[controller]/[action]/PostXmlData")]
        public HttpResponseMessage PostData()
        {
            BeginDataInsertIf();

            return new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        [HttpGet]
        [Route("/[controller]/[action]/GetTransactionData")]
        public ITransactions GetTransactionStatistics([FromQuery] ITransactionsResponse _transactionsResponse)
        {
            return _transactionsResponse.CollectTransactions();
        }
    }
}