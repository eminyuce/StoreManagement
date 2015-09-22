using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManagement.Liquid.ScheduledTasks
{
    public interface IHttpContextFactory
    {
        HttpContextBase Create();
    }

    public class HttpContextFactory
        : IHttpContextFactory
    {
        public HttpContextBase Create()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }
    }
}