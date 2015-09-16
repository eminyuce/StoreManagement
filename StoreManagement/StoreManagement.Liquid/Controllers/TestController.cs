using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Services;

namespace StoreManagement.Liquid.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        public async Task<ActionResult> Index()
        {
            int? categoryId = null;
          //  var list = ProductService.GetProductsByBrandAsync(StoreId, 5, 100, null);
          //  var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "HomePage");
          //  //var blogsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
          //  //var newsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

          //  //Task.WaitAny(list);
          //  //Task.WaitAny(pageDesignTask);
          //  //Task.WaitAny(blogsTask);
          //  //Task.WaitAny(newsTask);

          ////  var list2 = await list;
          ////  var pageDesignTask2 = await pageDesignTask;
         
          //  Task.WhenAll(list, pageDesignTask).Wait();

            String ConnectionString = "Stores";

            IProductService rep = new ProductRepository(new StoreContext(ConnectionString));
            IContentService rep2 = new ContentRepository(new StoreContext(ConnectionString));
            IPageDesignService rep3 = new PageDesignRepository(new StoreContext(ConnectionString));


            var list = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

            Task.WhenAll(list, pageDesignTask, blogsTask, newsTask).Wait();


            return View(list.Result);
        }

        public async Task<ActionResult> Index2()
        {
            int? categoryId = null;
          

            String ConnectionString = "Stores";
            var webServiceAddress = ProjectAppSettings.GetWebConfigString("WebServiceAddress", "localhost:8164");
            IProductService rep = new ProductService(webServiceAddress);
            IContentService rep2 = new ContentService(webServiceAddress);
            IPageDesignService rep3 = new PageDesignService(webServiceAddress);


            var list = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

            Task.WhenAll(list, pageDesignTask, blogsTask, newsTask).Wait();


            return View(list.Result);
        }
	}
}