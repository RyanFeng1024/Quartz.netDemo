using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Quartz.Net.Demo.Codes
{
    public class QuartzUtil
    {
        private static ISchedulerFactory sf = null;
        private static IScheduler sched = null;

        static QuartzUtil()
        {

        }

        public static async void Init()
        {
            sf = new StdSchedulerFactory();
            sched = await sf.GetScheduler();
            await sched.Start();
        }

        /// <summary>
        /// 添加Job 并且以定点的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="CronTime"></param>
        /// <param name="jobDataMap"></param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, string CronTime, Dictionary<string, object> map) where T : IJob
        {
            IJobDetail jobCheck = JobBuilder.Create<T>().WithIdentity(JobName, JobName + "_Group").Build();
            if (map != null)
            {
                jobCheck.JobDataMap.PutAll(map);
            }
            ICronTrigger CronTrigger = new CronTriggerImpl(JobName + "_CronTrigger", JobName + "_TriggerGroup", CronTime);
            await sched.ScheduleJob(jobCheck, CronTrigger);
        }

        /// <summary>
        /// 添加Job 并且以定点的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="CronTime"></param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, string CronTime) where T : IJob
        {
            await AddJob<T>(JobName, CronTime, null);
        }

        /// <summary>
        /// 添加Job 并且以周期的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="SimpleTime">毫秒数</param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, DateTimeOffset StartTime, DateTimeOffset EndTime, int SimpleTime) where T : IJob
        {
            await AddJob<T>(JobName, StartTime, EndTime, TimeSpan.FromMilliseconds(SimpleTime));
        }

        /// <summary>
        /// 添加Job 并且以周期的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="SimpleTime"></param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan SimpleTime) where T : IJob
        {
            await AddJob<T>(JobName, StartTime, EndTime, SimpleTime, new Dictionary<string, object>());
        }

        /// <summary>
        /// 添加Job 并且以周期的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="SimpleTime">毫秒数</param>
        /// <param name="jobDataMap"></param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, DateTimeOffset StartTime, DateTimeOffset EndTime, int SimpleTime, string MapKey, object MapValue) where T : IJob
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add(MapKey, MapValue);
            await AddJob<T>(JobName, StartTime, EndTime, TimeSpan.FromMilliseconds(SimpleTime), map);
        }

        /// <summary>
        /// 添加Job 并且以周期的形式运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JobName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="SimpleTime"></param>
        /// <param name="jobDataMap"></param>
        /// <returns></returns>
        public static async Task AddJob<T>(string JobName, DateTimeOffset StartTime, DateTimeOffset EndTime, TimeSpan SimpleTime, Dictionary<string, object> map) where T : IJob
        {
            IJobDetail jobCheck = JobBuilder.Create<T>().WithIdentity(JobName, JobName + "_Group").Build();
            jobCheck.JobDataMap.PutAll(map);
            ISimpleTrigger triggerCheck = new SimpleTriggerImpl(JobName + "_SimpleTrigger", JobName + "_TriggerGroup",
                                        StartTime,
                                        EndTime,
                                        SimpleTriggerImpl.RepeatIndefinitely,
                                        SimpleTime);
            await sched.ScheduleJob(jobCheck, triggerCheck);
        }

        /// <summary>
        /// 修改触发器时间,需要job名,以及修改结果
        /// CronTriggerImpl类型触发器
        /// </summary>
        public static async void UpdateTime(string jobName, string CronTime)
        {
            TriggerKey TKey = new TriggerKey(jobName + "_CronTrigger", jobName + "_TriggerGroup");
            CronTriggerImpl cti = await sched.GetTrigger(TKey) as CronTriggerImpl;
            cti.CronExpression = new CronExpression(CronTime);
            await sched.RescheduleJob(TKey, cti);
        }

        /// <summary>
        /// 修改触发器时间,需要job名,以及修改结果
        /// SimpleTriggerImpl类型触发器
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="SimpleTime">分钟数</param>
        public static void UpdateTime(string jobName, int SimpleTime)
        {
            UpdateTime(jobName, TimeSpan.FromMinutes(SimpleTime));
        }

        /// <summary>
        /// 修改触发器时间,需要job名,以及修改结果
        /// SimpleTriggerImpl类型触发器
        /// </summary>
        public static async void UpdateTime(string jobName, TimeSpan SimpleTime)
        {
            TriggerKey TKey = new TriggerKey(jobName + "_SimpleTrigger", jobName + "_TriggerGroup");
            SimpleTriggerImpl sti = await sched.GetTrigger(TKey) as SimpleTriggerImpl;
            sti.RepeatInterval = SimpleTime;
            await sched.RescheduleJob(TKey, sti);
        }

        /// <summary>
        /// 暂停所有Job
        /// 暂停功能Quartz提供有很多,以后可扩充
        /// </summary>
        public static void PauseAll()
        {
            sched.PauseAll();
        }

        /// <summary>
        /// 恢复所有Job
        /// 恢复功能Quartz提供有很多,以后可扩充
        /// </summary>
        public static void ResumeAll()
        {
            sched.ResumeAll();
        }

        /// <summary>
        /// 删除Job
        /// 删除功能Quartz提供有很多,以后可扩充
        /// </summary>
        /// <param name="JobName"></param>
        public static void DeleteJob(string JobName)
        {
            JobKey jk = new JobKey(JobName, JobName + "_Group");
            sched.DeleteJob(jk);
        }

        /// <summary>
        /// 卸载定时器
        /// </summary>
        /// <param name="waitForJobsToComplete">是否等待job执行完成</param>
        public static void Shutdown(bool waitForJobsToComplete)
        {
            if (sched != null)
            {
                sched.Shutdown(waitForJobsToComplete);
            }
        }

        /// <summary>
        /// 判断任务是否已经建立
        /// </summary>
        /// <param name="jobName">任务名</param>
        public static async Task<bool> CheckExist(string jobName)
        {
            bool isExists = false;

            TriggerKey triggerKey = new TriggerKey(jobName + "_CronTrigger", jobName + "_TriggerGroup");
            isExists = await sched.CheckExists(triggerKey);

            return isExists;
        }

        /// <summary>
        /// 判断简单任务是否已经建立
        /// </summary>
        /// <param name="jobName">任务名</param>
        public static async Task<bool> CheckSimpleExist(string jobName)
        {
            bool isExists = false;

            TriggerKey triggerKey = new TriggerKey(jobName + "_SimpleTrigger", jobName + "_TriggerGroup");
            isExists = await sched.CheckExists(triggerKey);

            return isExists;
        }
    }
}