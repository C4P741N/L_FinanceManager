using Microsoft.AspNetCore.Mvc;
using MSota.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using DataLibrary;
using static DataLibrary.BusinessLogic.StatisticsProcessor;
using static BaseCommands.B_BaseCommands;
using DataAndStatistics;
using LogicObjects;

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

            return View();
        }
        public IActionResult ViewDataAndStatistics()
        {
            double AmountBorrowed = 0;
            double AmountCharged  = 0;
            double AmountReceived = 0;
            double AmountSpent = 0;


            ViewBag.Message = "Statistics List";

            U_StatisticsProp statprop = null;
            L_Recepients rps = null;

            BeginLaunchOfStuffToGetData(ref rps);

            var statistics = new List<StatisticsModel>();

            //statistics.Add(new StatisticsModel
            //{
            //    AmountBorrowed = statprop.FulizaAmount,
            //    AmountCharged = statprop.FulizaCharge,
            //    AmountReceived = statprop.CashReceived,
            //    AmountSpent = statprop.CashSpent
            //});

            foreach (L_Recepient rp in rps)
            {
                foreach (var row in rp.transactions) //Useful for list row
                {

                    AmountBorrowed = row.TotalFulizaBorrowed;
                    AmountCharged = row.TotalFulizaCharge;
                    AmountReceived = row.TotalTransactionDeposited;
                    AmountSpent = row.TotalTransactionWithdrawn;
                   
                }
                statistics.Add(new StatisticsModel
                {
                    ListReceipientNames = rp.RecepientName,
                    AmountBorrowed = AmountBorrowed,
                    AmountCharged  = AmountCharged ,
                    AmountReceived = AmountReceived,
                    AmountSpent = AmountSpent
                });
            }

            statistics.Sort((x, y) => string.Compare(x.ListReceipientNames, y.ListReceipientNames));

            return View(statistics);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}