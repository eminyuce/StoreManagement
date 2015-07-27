using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class BlogsController : BaseController
    {
        //
        // GET: /Blogs/
        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.BlogsType))
                {
                    return HttpNotFound("Not Found");
                }

                var blogsPageDesignTask = PageDesignService.GetPageDesignByName(Store.Id, "BlogsIndex");
                var contentsTask = ContentService.GetContentsCategoryIdAsync(Store.Id, null, StoreConstants.BlogsType, true, page, GetSettingValueInt("BlogsIndexPageSize", StoreConstants.DefaultPageSize));
                var categories = CategoryService.GetCategoriesByStoreIdAsync(Store.Id, StoreConstants.BlogsType, true);
                Task.WaitAll(blogsPageDesignTask, contentsTask, categories);
                var dic = BlogHelper.GetBlogsIndexPage(this.HttpContext.Request, contentsTask, blogsPageDesignTask, categories);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                return new HttpStatusCodeResult(500);
            }
        }


        //
        // GET: /Blogs/
        public ActionResult Index2()
        {
            return View();
        }

        //
        // GET: /Blogs/
        public ActionResult Blog()
        {
            return View();
        }
    }
}