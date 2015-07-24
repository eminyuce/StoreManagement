using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManagement.Admin.ScheduledTasks
{
    public interface IBaseTasksScheduler
    {
        void Start();
        void Stop();
    }
}