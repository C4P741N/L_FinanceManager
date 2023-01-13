using DataAndStatistics;
using LogicObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSota.Models;
using static BaseCommands.B_BaseCommands;

namespace MSota.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/[controller]/[action]/PostData")]
        public HttpResponseMessage PostData()
        {
            //bc = new B_BaseCommands();

            //bc.BeginLaunchOfXMLStuff();
            //B_BaseCommands.BeginDataInsertIf();

            return new HttpResponseMessage{
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        [HttpGet]
        [Route("/[controller]/[action]/GetTransactionData")]
        public L_Transactions GetTransactionStatistics()
        {
            return Transactions();
        }
    }
}