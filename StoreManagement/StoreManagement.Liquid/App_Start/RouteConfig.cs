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
                 url: "sitemaps/products.xml",
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
                     name: "ProductsDetail",
                     url: "Products/Product/{categoryName}/{id}",
                     defaults: new { controller = "Products", action = "Product", id = UrlParameter.Optional }
                 );


            routes.MapRoute(
               name: "ProductsSearch",
               url: "Products/{id}/{*filters}",
         defaults: new { controller = "Products", action = "Index", filters = UrlParameter.Optional, id = UrlParameter.Optional }
         , constraints: new { filters = @"(^[^/]+-.*)|^$" }
         );
            //AjaxProducts
            routes.MapRoute(
             name: "AjaxProducts",
             url: "AjaxProducts/{action}",
             defaults: new { controller = "AjaxProducts", action = "GetProductsByProductType" }
         );

            routes.MapRoute(
             name: "Css",
             url: "Css/{action}",
             defaults: new { controller = "Css", action = "GetTheme" }
         );


            routes.MapRoute(
             name: "test",
             url: "test/{action}",
             defaults: new { controller = "test", action = "index4" }
         );

            routes.MapRoute(
             name: "Rss",
             url: "Rss/{action}",
             defaults: new { controller = "Rss", action = "Products" }
         );

            routes.MapRoute(
               name: "ImageUrl",
               url: "Images/ImageUrl/{id}",
               defaults: new { controller = "Images", action = "ImageUrl", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "ProductAction",
                url: "Products/{action}/{id}",
                defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{id}",
                defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
