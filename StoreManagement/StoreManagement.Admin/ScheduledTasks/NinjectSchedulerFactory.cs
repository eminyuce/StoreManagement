using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;

namespace StoreManagement.Admin.ScheduledTasks
{
    public class NinjectSchedulerFactory : StdSchedulerFactory
    {
        private readonly NinjectJobFactory _ninjectJobFactory;

        public NinjectSchedulerFactory(NinjectJobFactory ninjectJobFactory)
        {
            _ninjectJobFactory = ninjectJobFactory;
        }

        protected override IScheduler Instantiate(global::Quartz.Core.QuartzSchedulerResources rsrcs, global::Quartz.Core.QuartzScheduler qs)
        {
            qs.JobFactory = _ninjectJobFactory;
            return base.Instantiate(rsrcs, qs);
        }
    }

}