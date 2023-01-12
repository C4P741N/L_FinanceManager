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

        //public HttpResponse ExtractAndAddDataOnClick()
        //{
        //    //bc = new B_BaseCommands();

        //    //bc.BeginLaunchOfXMLStuff();
        //    B_BaseCommands.BeginDataInsertIf();

        //     return null;
        //}

        [HttpGet]
        [Route("/[controller]/[action]/GetData")]
        public List<StatisticsModel> ViewDataAndStatistics()
        {
            var statistics = new List<StatisticsModel>();

            foreach (L_Recepient rp in BeginLaunchOfStuffToGetData())
            {
                foreach (var row in rp.transactions) //Useful for list row
                {

                    statistics.Add(new StatisticsModel
                    {
                        ListReceipientNames = row.TotalFulizaBorrowed.ToString(),
                        AmountBorrowed = row.TotalFulizaCharge,
                        AmountReceived = row.TotalTransactionDeposited,
                        AmountSpent = row.TotalTransactionWithdrawn
                    });
                }
            }

            return statistics;
        }
    }
}