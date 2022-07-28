using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseCommands;

namespace MSota.Controllers
{
    public class ButtonClick : Controller
    {
        private B_BaseCommands bc = null;
        public IActionResult ExtractAndAddDataOnClick()
        {
            bc = new B_BaseCommands();

            bc.BeginLaunchOfXMLStuff();

            return View("/index.cshtml");
        }
        public void RetriveDataOnClick()
        {
            bc = new B_BaseCommands();

            bc.BeginLaunchOfStuffToGetData();
        }

    }
}
