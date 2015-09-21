using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using Quartz;
using StoreManagement.Liquid.ScheduledTasks.Jobs;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Liquid.ScheduledTasks
{
    public class LiquidTasksScheduler : BaseTasksScheduler
    {
        public IScheduler Scheduler { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [Inject]
        public IStoreService StoreService { set; get; }


        public LiquidTasksScheduler(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public override void Start()
        {
            Logger.Trace("TestJob is LiquidTasksScheduler running.");
            var m = JobBuilder.Create<TestJob>();

            IJobDetail testJob = m.Build();


            var trigger = TriggerBuilder.Create()
                .WithIdentity("DeleteGoogleDriveFiles", "DeleteGoogleDriveFiles")
                .WithCalendarIntervalSchedule(x => x.WithIntervalInHours(1))
                .WithDescription("trigger")
                .StartNow()
                .Build();


            Scheduler.ScheduleJob(testJob, trigger);

            Scheduler.Start();

        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}