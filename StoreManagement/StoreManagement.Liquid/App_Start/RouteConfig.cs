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
                name: "storeRetailersSiteMap",
                url: "sitemaps/retailers.xml",
                        defaults: new { controller = "Sitemaps", action = "Retailers" }
             );

             routes.MapRoute(
                name: "storeBrandsSiteMap",
                url: "sitemaps/brands.xml",
                        defaults: new { controller = "Sitemaps", action = "Brands" }
             );

             routes.MapRoute(
                 name: "storeProductSitemap",
                 url: "sitemaps/productssitemap.xml",
                         defaults: new { controller = "Sitemaps", action = "Products" }
              );

             routes.MapRoute(
                name: "storeProductCategoriesSiteMap",
                url: "sitemaps/productcategories.xml",
                        defaults: new { controller = "Sitemaps", action = "ProductCategories" }
             );

            routes.MapRoute(
             name: "robots",
             url: "robots.txt",
                     defaults: new { controller = "Robots", action = "RobotsText" }
          );

            routes.MapRoute(
               name: "ProductsSearch",
               url: "Products/Index/{id}/{*filters}",
         defaults: new { controller = "Products", action = "Index", filters = UrlParameter.Optional, id = UrlParameter.Optional }
         , constraints: new { filters = @"(^[^/]+-.*)|^$" }
         );

            routes.MapRoute(
                      name: "ProductsDetail",
                      url: "Products/Product/{categoryName}/{id}",
                      defaults: new { controller = "Products", action = "Product", id = UrlParameter.Optional }
                  );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "products", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
