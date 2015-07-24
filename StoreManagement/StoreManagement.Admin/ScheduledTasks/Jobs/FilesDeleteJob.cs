using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Quartz;

namespace StoreManagement.Admin.ScheduledTasks.Jobs
{
    public class FilesDeleteJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();



        public void Execute(IJobExecutionContext context)
        {

            try
            {
                Logger.Info(String.Format("Test 1"));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


        }
    }
}