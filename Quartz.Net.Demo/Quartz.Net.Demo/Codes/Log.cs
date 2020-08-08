using log4net;
using System;

namespace Quartz.Net.Demo.Codes
{
    public class Log
    {
        private static ILog logs = LogManager.GetLogger("File");

        public static void Error(object obj, Exception e)
        {
            logs.Error(obj.ToString(), e);
        }

        public static void Debug(object obj, Exception e)
        {
            logs.Debug(obj.ToString(), e);
        }

        public static void Info(object obj)
        {
            logs.Info(obj.ToString());
        }
    }
}