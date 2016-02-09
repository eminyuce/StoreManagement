using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEntities;


namespace StoreManagement.Liquid.Controllers
{ 
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class HomeController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public async Task<ActionResult> Index2()
        {


            int sliderTake = GetSettingValueInt("HomePageSliderImages_ItemsNumber", StoreConstants.DefaultPageSize);

            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "HomePage");
            var sliderTask = FileManagerService.GetStoreCarouselsAsync(StoreId, sliderTake);
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();


            await Task.WhenAll(pageDesignTask, sliderTask);

            var settings = GetStoreSettings();


            var pageDesing = pageDesignTask.Result;
            var sliderImages = sliderTask.Result;


            StoreLiquidResult liquidResult = ProductService2.GetHomePageDesign(pageDesing, sliderImages);
          
            liquidResult.PageTitle = GetSettingValue("HomePage_Title", "");
            liquidResult.StoreSettings = settings;
            liquidResult.MyStore = this.MyStore;

            // Stop timing.
            stopwatch.Stop();

            Logger.Info("Home:Index:Time elapsed: {0} elapsed milliseconds", stopwatch.ElapsedMilliseconds);
            return View(liquidResult);

        }


        public async Task<ActionResult> Index()
        {


            int blogsTake = GetSettingValueInt("HomePageMainBlogsContents_ItemsNumber", StoreConstants.DefaultPageSize);
            int newsTake = GetSettingValueInt("HomePageMainNewsContents_ItemsNumber", StoreConstants.DefaultPageSize);
            int productsTake = GetSettingValueInt("HomePageMainProductsContents_ItemsNumber", StoreConstants.DefaultPageSize);
            int sliderTake = GetSettingValueInt("HomePageSliderImages_ItemsNumber", StoreConstants.DefaultPageSize);


            int? categoryId = null;
            String key = String.Format("Home:Index-{0}-{1}-{2}-{3}-{4}", StoreId, blogsTake, newsTake,
                                       productsTake, sliderTake);

            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "HomePageWithMainData");
            var blogsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, blogsTake);
            var newsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, newsTake);
            var productsTask = ProductService.GetMainPageProductsAsync(StoreId, productsTake);
            var sliderTask = FileManagerService.GetStoreCarouselsAsync(StoreId, sliderTake);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, "", true);
            var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);

            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();



            await Task.WhenAll(productsTask, blogsTask, newsTask, pageDesignTask, sliderTask, categoriesTask,
                     productCategoriesTask);
            var settings = GetStoreSettings();

            var products = productsTask.Result;
            var blogs = blogsTask.Result;
            var news = newsTask.Result;
            var pageDesing = pageDesignTask.Result;
            var sliderImages = sliderTask.Result;
            var categories = categoriesTask.Result;
            var productCategories = productCategoriesTask.Result;

            StoreLiquidResult liquidResult = ProductService2.GetHomePageDesign(pageDesing, sliderImages, products, blogs,
                                                                              news, categories, productCategories);
            liquidResult.PageTitle = GetSettingValue("HomePage_Title", "");
            liquidResult.StoreSettings = settings;
            liquidResult.MyStore = this.MyStore;


            // Stop timing.
            stopwatch.Stop();

            Logger.Info("Home:Index:Time elapsed: {0} elapsed milliseconds", stopwatch.ElapsedMilliseconds);
            return View(liquidResult);


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

        //[ChildActionOnly]
        //public ActionResult MainLayoutJavaScriptFiles()
        //{
        //    int storeId = StoreId;
        //    var pageDesignTask1 = PageDesignService.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
        //    var pageDesignTask2 = PageDesignService.GetPageDesignByName(storeId, "MainLayoutCssFiles");
        //    Task.WaitAll(pageDesignTask1, pageDesignTask2);

        //    if (pageDesignTask1.Result == null)
        //    {
        //        return Content("MainLayoutJavaScriptFiles pageDesign is null");
        //    }
        //    if (pageDesignTask2.Result == null)
        //    {
        //        return Content("MainLayoutCssFiles pageDesign is null");
        //    }


        //    var contentFiles = pageDesignTask1.Result.PageTemplate.HtmlDecode();
        //    contentFiles += pageDesignTask2.Result.PageTemplate.HtmlDecode();
        //    return Content(contentFiles);
        //}

    }
}