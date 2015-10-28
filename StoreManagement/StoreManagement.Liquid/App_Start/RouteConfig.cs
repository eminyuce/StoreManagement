using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StoreManagement.Liquid
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



             routes.MapRoute(
                 name: "storeDefaultSitemap",
                 url: "sitemaps/sitemap.xml",
                         defaults: new { controller = "Sitemaps", action = "Index" }
              );
             routes.MapRoute(
                 name: "storeProductSitemap",
                 url: "sitemaps/productssitemap.xml",
                         defaults: new { controller = "Sitemaps", action = "Products" }
              );

            routes.MapRoute(
             name: "robots",
             url: "robots.txt",
                     defaults: new { controller = "Robots", action = "RobotsText" }
          );


            routes.MapRoute(
                      name: "ProductsDetail",
                      url: "Products/Product/{categoryName}/{id}",
                      defaults: new { controller = "Products", action = "Product", id = UrlParameter.Optional }
                  );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
