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
            ViewBag.Store = Store;
            var shp =new StoreHomePage();
            shp.Store = Store;
            shp.CarouselImages = StoreCarouselService.GetStoreCarousels(Store.Id);
            shp.Categories = CategoryService.GetCategoriesByStoreId(Store.Id);
            var m = ContentService.GetContentsCategoryId(Store.Id, null, "product", true, page, 24);
            shp.Products = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            m = ContentService.GetContentsCategoryId(Store.Id, null, "news", true, page, 24);
            shp.News = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            m = ContentService.GetContentsCategoryId(Store.Id, null, "blog", true, page, 24);
            shp.Blogs = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
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
