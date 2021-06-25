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
            string jobs = QuartzUtil.PrintAllJobsInfo().Result;
            ViewBag.AllJobs = jobs.Replace("\n", "<br />");
            return View();
        }

        public ActionResult AddJob()
        {
            int intervalSecs = 10;
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("job_data", "add the data you want to use in the job");
            QuartzUtil.AddSimpleJob<TestJob>("TEST_JOB_" + DateTime.Now.ToString("HHmmss"), intervalSecs, map);
            return Redirect("/");
        }

        public ActionResult DeleteJob()
        {
            string jobName = "job1";
            QuartzUtil.DeleteJob(jobName);
            return Redirect("/");
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