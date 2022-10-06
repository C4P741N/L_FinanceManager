using Microsoft.AspNetCore.Mvc;
using MSota.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using DataLibrary;
using static DataLibrary.BusinessLogic.StatisticsProcessor;
using static BaseCommands.B_BaseCommands;
using DataAndStatistics;

namespace MSota.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //AddStatisticsToDb();

            return View();
        }

        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult DataAndStatistics()
        {

            return View();
        }

        public IActionResult InsertDataAndStatisticsIf()
        {
            BeginDataInsertIf();

            return ViewDataAndStatistics();
        }
        public IActionResult ViewDataAndStatistics()
        {
            ViewBag.Message = "Statistics List";

            U_StatisticsProp statprop = null;

            BeginLaunchOfStuffToGetData(ref statprop);

            var statistics = new List<StatisticsModel>();

            statistics.Add(new StatisticsModel
            {
                AmountBorrowed = statprop.FulizaAmount,
                AmountCharged = statprop.FulizaCharge,
                AmountReceived = statprop.CashReceived,
                AmountSpent = statprop.CashSpent
            });

            //foreach (var row in data) //Useful for list row
            //{
            //    statistics.Add(new StatisticsModel
            //    {
            //        AmountBorrowed = row.AmountBorrowed,
            //        AmountCharged = row.AmountCharged,
            //        AmountReceived = row.AmountReceived,
            //        AmountSpent = row.AmountSpent
            //    });
            //}

            return View(statistics);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}