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
    [DisallowConcurrentExecution]   //拒绝同一时间重复执行，同一任务串行
    public class GetRecordJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var count = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
            double memoryUsage = (((double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64) / 1024) / 1024;

            Log.Info("********************* JOB 【" + context.JobDetail.Key + "】执行开始 *********************");
            Log.Info("当前线程ID：" + Thread.CurrentThread.ManagedThreadId + " | 共有线程数量：" + count + " | 当前使用内存：" + memoryUsage + "MB");

            Task[] tasks = new Task[1];
            tasks[0] = Task.Factory.StartNew(() =>
            {
                //模拟程序运行一段时间卡死的情况
                if (memoryUsage >= 120) //当程序运行内存达到120MB时，发送一个响应时间较长的请求
                {
                    Log.Info("开始模拟延迟请求 20s返回结果");
                    string host = System.Configuration.ConfigurationManager.AppSettings["WebUrl"].ToString();
                    string url = host.TrimEnd('/') + "/Home/Delay";
                    Log.Info("请求接口获取数据 : " + url);
                    var result = HttpUtils.HttpGet(url);
                    Log.Info("接口 result:" + result);
                }
                else
                {
                    Log.Info("内存正常，直接返回");
                }
            });
            Task.WaitAll(tasks, 10000);     //Task等待10后，判断Task状态。假设上段代码正常情况下 预期10s内应该执行完成，如果10s后 Task 还未完成，就当做异常处理 执行重启命令，防止一直卡死 后面的定时任务不执行，
            if (tasks[0].Status != TaskStatus.RanToCompletion)
            {
                Log.Info("Task Error! 超出10s未响应. 准备重启");
                MvcApplication.Restart();
                Log.Info("Task Error! 超出10s未响应. 重启完毕");
            }

            Log.Info("********************* JOB 【" + context.JobDetail.Key + "】执行结束 *********************");
        }
    }
}