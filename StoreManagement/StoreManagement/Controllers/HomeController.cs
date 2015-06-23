using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Constants;
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
            var shp = new StoreHomePage();
            try
            {

                shp.Store = Store;
                shp.CarouselImages = FileManagerService.GetStoreCarousels(Store.Id);
                shp.ProductCategories = ProductCategoryService.GetProductCategoriesByStoreId(Store.Id);
                var products = ProductService.GetProductsCategoryId(Store.Id, null, StoreConstants.ProductType, true, page, 24);
                shp.Products = new PagedList<Product>(products.items, products.page - 1, products.pageSize, products.totalItemCount);
                var contents = ContentService.GetContentsCategoryId(Store.Id, null, StoreConstants.NewsType, true, page, 24);
                shp.News = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
                contents = ContentService.GetContentsCategoryId(Store.Id, null, StoreConstants.BlogsType, true, page, 24);
                shp.Blogs = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
            }
            catch (Exception ex)
            {

                Logger.ErrorException("Home page exception" + ex.Message, ex);
            }

            return View(shp);
        }
        public ActionResult About()
        {
            String  key = "AboutUs";
            var item = SettingService.GetStoreSettingsFromCache(Store.Id).FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return View(item);
        }
        public ActionResult Contact()
        {
            String key = "Contacts";
            var item = SettingService.GetStoreSettingsFromCache(Store.Id).FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return View(item);
        }
        public ActionResult Locations()
        {
            String key = "Location";
            var item = SettingService.GetStoreSettingsFromCache(Store.Id).FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return View(item);
        }
        public ActionResult TermsAndCondition()
        {
            String key = "TermsAndCondition";
            var item = SettingService.GetStoreSettingsFromCache(Store.Id).FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return View(item);
        }
        public ActionResult Footer()
        {
            String key = "Footer";
            var item = SettingService.GetStoreSettingsFromCache(Store.Id).FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return View(item);
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
       
        public ActionResult Test()
        {
            return View();
        }
    }
}
