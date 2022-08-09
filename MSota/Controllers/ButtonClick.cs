﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseCommands;
using MSota.Models;
using DataAndStatistics;

namespace MSota.Controllers
{
    public class ButtonClick : Controller
    {
        private DataAndStatistics.DataAndStatisticsProp dsprop = null;
        private Statement st = null;
        private StatementController stctr = null;
        private B_BaseCommands bc = null;
        private U_StatisticsProp uisprop = null;
        public IActionResult ExtractAndAddDataOnClick()
        {
            bc = new B_BaseCommands();

            bc.BeginLaunchOfXMLStuff();

            return View();
        }
        public void RetriveDataOnClick()
        {
            bc = new B_BaseCommands();
            st = new Statement();
            stctr = new StatementController();

            bc.BeginLaunchOfStuffToGetData(ref uisprop);

            //stctr.Dispose();

            stctr.StatementDetails(ref uisprop);
        }

    }
}
