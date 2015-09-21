using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using Quartz;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Liquid.ScheduledTasks.Jobs
{
    public class TestJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

       
        public void Execute(IJobExecutionContext context)
        {
            Logger.Trace("TestJob is ScheduledTasks running.");
        }
    }
}