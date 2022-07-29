using Microsoft.AspNetCore.Mvc;
using MSota.Models;
using System.Collections.Generic;
//using System.Web.Mvc;

namespace MSota.Controllers
{
    public class StatementController : Controller
    {
        private DataAndStatistics.DataAndStatisticsProp dsprop = null;
        private Statement st = null;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StatementDetails()
        {
            st = new Statement();

            st.AmountSpent = 56;
            st.AmountBorrrowed = 57;
            st.AmountReceived = 98;

            //ViewBag.StatementDetails = st;

            return View(st);
        }
    }
}
