using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    //[OutputCache(CacheProfile = "Cache1Days")]
    public class HomeController : BaseController
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {

            int page = 1;
            var shp = new StoreHomePage();
            try
            {

                shp.SStore = MyStore;
                shp.SCarouselImages = FileManagerService.GetStoreCarousels(MyStore.Id);
                shp.SProductCategories = ProductCategoryService.GetProductCategoriesByStoreId(MyStore.Id);
                var products = ProductService.GetProductsCategoryId(MyStore.Id, null, StoreConstants.ProductType, true, page, 24);
                shp.SProducts = new PagedList<Product>(products.items, products.page - 1, products.pageSize, products.totalItemCount);
                var contents = ContentService.GetContentsCategoryId(MyStore.Id, null, StoreConstants.NewsType, true, page, 24);
                shp.SNews = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
                contents = ContentService.GetContentsCategoryId(MyStore.Id, null, StoreConstants.BlogsType, true, page, 24);
                shp.SBlogs = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
            }
            catch (Exception ex)
            {

                Logger.Error(ex,"Home page exception" + ex.StackTrace);
            }

            return View(shp);
        }
        public ActionResult About()
        {
            var item = GetSettingValue(StoreConstants.AboutUs);
            return View(item);
        }
        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Locations()
        {

            return View();
        }
        public ActionResult TermsAndCondition()
        {
            var item = GetSettingValue(StoreConstants.TermsAndCondition);
            return View(item);
        }
        public ActionResult Footer()
        {
            var item = GetSettingValue(StoreConstants.Footer);
            return View(item);
        }
        public ActionResult RecentUpdates()
        {
            return View();
        }

        public ActionResult MainMenu()
        {
            var navigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            return View(navigations);
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult PageDesignTest()
        {
            

            return View();
        }


    }
}
