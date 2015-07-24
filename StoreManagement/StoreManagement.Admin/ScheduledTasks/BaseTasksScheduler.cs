using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManagement.Admin.ScheduledTasks
{
    public abstract class BaseTasksScheduler : IBaseTasksScheduler
    {
        public virtual void Start()
        {
             
        }

        public virtual void Stop()
        {
   
        }
    }
}