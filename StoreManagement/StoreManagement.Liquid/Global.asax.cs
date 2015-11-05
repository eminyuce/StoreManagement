using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotLiquid;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidFilters;

namespace StoreManagement.Liquid
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true; 
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Redirect301();
        }
        private void Redirect301()
        {

            if (Request.Url.Host.Contains("login.seatechnologyjobs.com"))
            {
               return;
            }

            if (!Request.Url.Host.StartsWith("www") && !Request.Url.IsLoopback)
            {
                UriBuilder builder = new UriBuilder(Request.Url);
                builder.Host = "www." + Request.Url.Host;
                Response.StatusCode = 301;
                Response.AddHeader("Location", builder.ToString());
                Response.End();
            }
        }
    }
}
