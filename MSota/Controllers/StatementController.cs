using Microsoft.AspNetCore.Mvc;
using MSota.Models;
using DataAndStatistics;
using System.Collections.Generic;
//using System.Web.Mvc;

namespace MSota.Controllers
{
    public class StatementController : Controller
    {
        private DataAndStatistics.DataAndStatisticsProp dsprop = null;
        private Statement st = null;
        private U_StatisticsProp uisprop = null;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StatementDetails(ref U_StatisticsProp uisprop)
        {
            st = new Statement();

            st.AmountReceived       = uisprop.CashReceived;
            st.AmountSpent          = uisprop.CashSpent;
            st.FulizaAmount        = uisprop.FulizaAmount;
            st.FulizaCharge        = uisprop.FulizaCharge;
            st.FulizaAmountPaid    = uisprop.FulizaAmountPaid;

            return View(st);
        }
    }
}
