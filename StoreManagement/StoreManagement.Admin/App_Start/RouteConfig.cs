using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StoreManagement.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "robots",
                url: "robots.txt",
                        defaults: new { controller = "Robots", action = "RobotsText" }
             );

           // routes.MapRoute(
           //name: "CompanySearch",
           //url: "Company/list/{*filters}",
           //defaults: new { controller = "Companies", action = "CompaniesSearch", filters = UrlParameter.Optional });

            routes.MapRoute(
          name: "CompanySearch",
          url: "Companies/CompaniesSearch/{*filters}",
          defaults: new { controller = "Companies", action = "CompaniesSearch", filters = UrlParameter.Optional });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}