using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        public async Task<ActionResult> Index()
        {
            int? categoryId = null;
            var list = ProductService.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = ContentService.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

            Task.WaitAny(list);
            Task.WaitAny(pageDesignTask);
            Task.WaitAny(blogsTask);
            Task.WaitAny(newsTask);

          //  var list2 = await list;
          //  var pageDesignTask2 = await pageDesignTask;
          // Task.WhenAll(list, pageDesignTask, blogsTask, newsTask).Wait();

            return View(list.Result);
        }
	}
}