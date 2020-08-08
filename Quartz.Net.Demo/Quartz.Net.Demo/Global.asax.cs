using log4net.Config;
using Quartz.Net.Demo.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Quartz.Net.Demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //应用程序启动时加载log4net设置 
            XmlConfigurator.Configure();

            Log.Info("Application_Start 触发");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // 初始化启动定时任务引擎
            QuartzUtil.Init();

            // 启动设定的任务
            QuartzBase.Start();

            Log.Info("Application_Start 启动设定的任务");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Log.Info("Application_End 触发\r\n");
            Activate();
        }

        public static void Restart()
        {
            Log.Info("Restart()  卸载所有定时器... ");
            QuartzUtil.Shutdown(false);     //true 等待当前job执行完再关闭，false 直接关闭
            Log.Info("Restart()  准备重启 ");
            HttpRuntime.UnloadAppDomain();  //会触发Application_End 事件
            Log.Info("Restart()  重启完成 ");

            //Activate();
        }


        /// <summary>
        /// “激活”程序 
        ///     IIS回收后，将触发Application_End事件，需发起一次请求才会触发 Application_Start事件开始执行定时任务
        /// </summary>
        public static void Activate()
        {
            string host = System.Configuration.ConfigurationManager.AppSettings["WebUrl"].ToString();
            string url = host.TrimEnd('/') + "/Home/Ping";
            Log.Info("PING URL : " + url);
            string res = HttpUtils.HttpGet(url);
            Log.Info("PING RESULT:" + res + "\r\n");
        }
    }
}
