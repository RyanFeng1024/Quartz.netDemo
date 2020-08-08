using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quartz.Net.Demo.Codes
{
    public class QuartzBase
    {
        public static async void Start()
        {
            await QuartzUtil.AddJob<GetRecordJob>("job1", "0/5 * * * * ?");     //5s执行一次

            //如果有多个任务 在这里添加..
        }
    }
}