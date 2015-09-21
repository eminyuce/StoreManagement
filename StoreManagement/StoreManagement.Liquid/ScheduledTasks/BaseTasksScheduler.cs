using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManagement.Liquid.ScheduledTasks
{
    public abstract class BaseTasksScheduler : IBaseTasksScheduler
    {
        public abstract void Start();
        public abstract void Stop();
    }
}