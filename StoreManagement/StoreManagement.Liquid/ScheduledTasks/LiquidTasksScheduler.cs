using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using Quartz;
using StoreManagement.Liquid.ScheduledTasks.Jobs;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Liquid.ScheduledTasks
{
    public class LiquidTasksScheduler : BaseTasksScheduler
    {
        public IScheduler Scheduler { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [Inject]
        public IStoreGeneralRepository  StoreService { set; get; }


        [Inject]
        private IHttpContextFactory HttpContextFactory { set; get; }


        public LiquidTasksScheduler(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public override void Start()
        {

          


            JobBuilder jobBuilder = JobBuilder.Create<TestJob>();
            

            IJobDetail testJob = jobBuilder.Build();


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