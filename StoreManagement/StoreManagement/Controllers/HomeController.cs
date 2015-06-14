using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            int page = 1;
 
            var shp =new StoreHomePage();
            shp.Store = Store;
            shp.CarouselImages = FileManagerService.GetStoreCarousels(Store.Id);
            shp.Categories = CategoryService.GetCategoriesByStoreId(Store.Id);
            var products = ProductService.GetProductsCategoryId(Store.Id, null, "product", true, page, 24);
            shp.Products = new PagedList<Product>(products.items, products.page - 1, products.pageSize, products.totalItemCount);
            var contents = ContentService.GetContentsCategoryId(Store.Id, null, "news", true, page, 24);
            shp.News = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
            contents = ContentService.GetContentsCategoryId(Store.Id, null, "blog", true, page, 24);
            shp.Blogs = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
            return View(shp);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult RecentUpdates()
        {
           return View();
        }
        public ActionResult MainMenu()
        {
            var mainMenu = NavigationService.GetStoreActiveNavigations(Store.Id);
            return View(mainMenu);
        }
        public ActionResult Footer()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
