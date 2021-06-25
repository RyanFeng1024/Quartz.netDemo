using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using System.Threading;

namespace Quartz.Net.Demo.Codes
{
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap map = context.JobDetail.JobDataMap;
            string job_data = map.Get("job_data").ToString();

            Log.Info(" test add a job when app running... and get data from jobDataMap :" + job_data);
        }
    }
}