using Quartz.Net.Demo.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Quartz.Net.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Restart()
        {
            MvcApplication.Restart();
            return Content("ok");
        }

        public ActionResult WebUrl()
        {
            string host = System.Configuration.ConfigurationManager.AppSettings["WebUrl"].ToString();
            return Content(host);
        }

        public ActionResult Ping()
        {
            return Content("Pong");
        }

        public ActionResult Delay()
        {
            Thread.Sleep(20000);
            return Content("ok");
        }
    }
}