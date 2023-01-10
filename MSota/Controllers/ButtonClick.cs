using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
using BaseCommands;
using MSota.Models;
using DataAndStatistics;
using LogicObjects;
using System.Web.Http;
using System.Net.Http;
//using Azure.Core;
//using Azure;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using System.Globalization;

namespace MSota.Controllers
{
    //[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [System.Web.Http.Route("api/[controller]")]
    [ApiController]

    public class ButtonClick : ApiController
    {
        private DataAndStatistics.DataAndStatisticsProp dsprop = null;
        private Statement st = null;
        private StatementController stctr = null;
        //private B_BaseCommands bc = null;
        private U_StatisticsProp uisprop = null;
        private L_Recepients xrps = null;


        //public HttpResponse ExtractAndAddDataOnClick()
        //{
        //    //bc = new B_BaseCommands();

        //    //bc.BeginLaunchOfXMLStuff();
        //    B_BaseCommands.BeginDataInsertIf();

        //     return null;
        //}

        [System.Web.Http.HttpGet]
        //public IActionResult RetriveDataOnClick()
        //{
        //    //bc = new B_BaseCommands();
        //    st = new Statement();
        //    stctr = new StatementController();

        //    B_BaseCommands.BeginLaunchOfStuffToGetData(ref xrps);
        //    //bc.BeginLaunchOfStuffToGetData(ref uisprop);

        //    //stctr.Dispose();
        //    stctr.StatementDetails(ref uisprop);

        //    return View("/Views/Statement/StatementDetails.cshtml");
        //}


        public IActionResult RetriveDataOnClick()
        {
            List<int> numbers = new List<int>()
            {
                4,
                6,
                8
            };

            return (IActionResult)Ok(numbers);
        }

        //[System.Web.Http.AcceptVerbs("GET")]
        //public HttpResponseMessage RetriveDataOnClick()
        //{
           
        //    st = new Statement()
        //    {
        //        AmountSpent= 0,
        //        FulizaAmount=200,
        //        FulizaBorrowed= 100,
        //    };

        //    List<int> numbers = new List<int>()
        //    {
        //        4,
        //        6,
        //        8
        //    };

        //    //HttpRequestMessage request;

        //    //HttpRequestMessage test;
        //    //test.Content;

        //    HttpResponseMessage httpResponse = Request.CreateResponse<List<int>>(HttpStatusCode.Accepted,
        //                                                               numbers);

        //    return httpResponse;


        //    //return new HttpResponseMessage()
        //    //{
        //    //    Content = new ObjectContent(st,
        //    //      Configuration.Formatters.JsonFormatter)
        //    //};

        //}

    }
}
