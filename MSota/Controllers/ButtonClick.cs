using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseCommands;

namespace MSota.Controllers
{
    public class ButtonClick : Controller
    {
        private B_BaseCommands bc = null;
        public void HandleButtonClick()
        {
            bc = new B_BaseCommands();

            bc.BeginLaunchOfStuff();
        }

    }
}
