using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Service.ApiServices;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;

namespace StoreManagement.Liquid.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        [AsyncTimeout(150)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<ActionResult> Index()
        {
         
            int? categoryId = null;

            String ConnectionString = "Stores";

            IProductService rep = new ProductRepository(new StoreContext(ConnectionString));
            IContentService rep2 = new ContentRepository(new StoreContext(ConnectionString));
            IPageDesignService rep3 = new PageDesignRepository(new StoreContext(ConnectionString));


            var list = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

             await  Task.WhenAll(list, pageDesignTask, blogsTask, newsTask);


            return View(list.Result);
        }

        [AsyncTimeout(150)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimeoutError")]
        public async Task<ActionResult> Index3()
        {
            int? categoryId = null;

            String ConnectionString = "Stores";

            IProductService rep = new ProductRepository(new StoreContext(ConnectionString));
            IContentService rep2 = new ContentRepository(new StoreContext(ConnectionString));
            IPageDesignService rep3 = new PageDesignRepository(new StoreContext(ConnectionString));
            IProductCategoryService rep4 = new ProductCategoryRepository(new StoreContext(ConnectionString));
            ICategoryService rep5 = new CategoryRepository(new StoreContext(ConnectionString));
            IFileManagerService rep6 = new FileManagerRepository(new StoreContext(ConnectionString));



            var productsTask = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);
            var productCategoriesTask = rep4.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            var categoriesTask = rep5.GetCategoriesByStoreIdAsync(StoreId,StoreConstants.NewsType,true);
            var sliderTask = rep6.GetStoreCarouselsAsync(StoreId, 58);




            //IProductService rep = ProductService;
            //IContentService rep2 = ContentService;
            //IPageDesignService rep3 = PageDesignService;
            //IProductCategoryService rep4 = ProductCategoryService;
            //ICategoryService rep5 = CategoryService;
            //IFileManagerService rep6 = FileManagerService;

            await Task.WhenAll(productsTask, blogsTask, newsTask, pageDesignTask, sliderTask, categoriesTask,
                     productCategoriesTask);

            HomePageHelper.StoreId = this.StoreId;
            HomePageHelper.StoreSettings = GetStoreSettings();

            var products = productsTask.Result;
            var blogs = blogsTask.Result;
            var news = newsTask.Result;
            var pageDesing = pageDesignTask.Result;
            var sliderImages = sliderTask.Result;
            var categories = categoriesTask.Result;
            var productCategories = productCategoriesTask.Result;

            StoreLiquidResult liquidResult = HomePageHelper.GetHomePageDesign(pageDesing, sliderImages, products, blogs,
                                                                              news, categories, productCategories);
            liquidResult.MyStore = this.MyStore;


            return View(liquidResult);
        }

        public async Task<ActionResult> Index2()
        {
            int? categoryId = null;


            String ConnectionString = "Stores";
            var webServiceAddress = ProjectAppSettings.GetWebConfigString("WebServiceAddress", "localhost:8164");
            IProductService rep = new ProductApiService(webServiceAddress);
            IContentService rep2 = new ContentApiService(webServiceAddress);
            IPageDesignService rep3 = new PageDesignApiService(webServiceAddress);


            var list = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

            await Task.WhenAll(list, pageDesignTask, blogsTask, newsTask);


            return View(list.Result);
        }

        public ActionResult Index4()
        {


            return View();
        }

    }
}