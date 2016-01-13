using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DotLiquid.NamingConventions;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Models;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            DotLiquid.Template.NamingConvention = new CSharpNamingConvention();

        }
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if ("User".Equals(custom, StringComparison.InvariantCultureIgnoreCase))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    return context.User.Identity.Name;
                }
                else
                {
                    return base.GetVaryByCustomString(context, custom);
                }
            }
            return base.GetVaryByCustomString(context, custom);
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