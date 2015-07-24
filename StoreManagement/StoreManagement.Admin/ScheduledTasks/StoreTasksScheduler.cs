using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Quartz;
using StoreManagement.Admin.ScheduledTasks.Jobs;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.ScheduledTasks
{
    public class StoreTasksScheduler : BaseTasksScheduler
    {

        public IScheduler Scheduler { get; set; }

        [Inject]
        public IStoreRepository StoreRepository { set; get; }


        public StoreTasksScheduler(IScheduler scheduler)
        {
            Scheduler = scheduler;
        }

        public override void Start()
        {

            var m = JobBuilder.Create<FilesDeleteJob>();
           
            IJobDetail testJob = m.Build();

            //ITrigger runOnce = TriggerBuilder.Create().WithSimpleSchedule(builder => builder.WithRepeatCount(0)).Build();

            ITrigger trigger = TriggerBuilder.Create()
                            .WithIdentity("trigger7", "group1")
                            .WithSimpleSchedule(x => x
                                .WithIntervalInSeconds(5)
                                .RepeatForever())
                            .EndAt(DateBuilder.DateOf(22, 0, 0))
                            .Build();


            Scheduler.ScheduleJob(testJob, trigger);

            Scheduler.Start();

        }

    }
}