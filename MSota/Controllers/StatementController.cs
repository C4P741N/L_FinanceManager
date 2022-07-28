using Microsoft.AspNetCore.Mvc;
using MSota.Models;
using System.Collections.Generic;
//using System.Web.Mvc;

namespace MSota.Controllers
{
    public class StatementController : Controller
    {
        private Statement st = null;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StatementDetails()
        {
            st = new Statement();

            st.AmountSpent = 70;
            st.AmountBorrrowed = 60;
            st.AmountReceived = 90;

            //ViewBag.StatementDetails = st;

            return View(st);
        }
    }
}
