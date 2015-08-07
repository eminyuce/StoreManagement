using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            try
            {
                int? categoryId = null;
                var pageDesignTask = PageDesignService.GetPageDesignByName(Store.Id, "HomePage");
                int take = GetSettingValueInt("HomePageMainBlogsContents_ItemsNumber", 5);
                var blogsTask = ContentService.GetMainPageContentsAsync(Store.Id, categoryId, StoreConstants.BlogsType, take);
                take = GetSettingValueInt("HomePageMainNewsContents_ItemsNumber", 5);
                var newsTask = ContentService.GetMainPageContentsAsync(Store.Id, categoryId, StoreConstants.NewsType, take);
                take = GetSettingValueInt("HomePageMainProductsContents_ItemsNumber", 5);
                var productsTask = ProductService.GetMainPageProductsAsync(Store.Id, take);
                take = GetSettingValueInt("HomePageSliderImages_ItemsNumber", 5);
                var sliderTask = FileManagerService.GetStoreCarouselsAsync(Store.Id, take);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(Store.Id);
                var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(Store.Id, StoreConstants.ProductType, true);
                var liquidHelper = new HomePageHelper();
                liquidHelper.StoreSettings = StoreSettings;
                var dic = liquidHelper.GetHomePageDesign(productsTask, blogsTask, newsTask, sliderTask, pageDesignTask, categoriesTask, productCategoriesTask);
                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "HomeController:Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult About()
        {
            var item = GetSettingValue(StoreConstants.AboutUs);
            return View(item);
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

        public ActionResult MainLayout()
        {
            int storeId = Store.Id;

            var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayout");
            var liquidHelper = new NavigationHelper();
            liquidHelper.StoreSettings = StoreSettings;
            var dic = liquidHelper.GetMainLayoutLink(mainMenu, pageDesignTask);
            return View(dic);

        }
        public ActionResult MainLayoutFooter()
        {
            int storeId = Store.Id;

            var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutFooter");
            var dic = NavigationHelper.GetMainLayoutFooterLink(mainMenu, pageDesignTask);
            return View(dic);

        }
        public ActionResult MainLayoutJavaScriptFiles()
        {
            int storeId = Store.Id;
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
            Task.WaitAll(pageDesignTask);
            return View(pageDesignTask.Result);

        }
        public ActionResult MainLayoutCssFiles()
        {
            int storeId = Store.Id;
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutCssFiles");
            Task.WaitAll(pageDesignTask);
            return View(pageDesignTask.Result);

        }

    }
}