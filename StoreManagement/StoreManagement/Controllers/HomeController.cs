using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Ninject;
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
            ViewBag.Store = Store;
            var shp =new StoreHomePage();
            shp.Store = Store;
            shp.CarouselImages = StoreCarouselService.GetStoreCarousels(Store.Id);
            shp.Categories = CategoryService.GetCategoriesByStoreId(Store.Id);
           
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
            var mainMenu = NavigationService.GetStoreNavigations(Store.Id);
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
