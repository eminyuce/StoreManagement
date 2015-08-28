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
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class BlogsController : BaseController
    {

        
        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.BlogsType))
                {
                    return HttpNotFound("Not Found");
                }
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
                var blogsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "BlogsIndex");
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, null, StoreConstants.BlogsType, true, page, GetSettingValueInt("BlogsIndex_PageSize", StoreConstants.DefaultPageSize));
                var categories = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.BlogsType, true);



                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("BlogsIndex_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("BlogsIndex_ImageHeight", 50);

                var pageOutput = ContentHelper.GetContentsIndexPage(contentsTask, blogsPageDesignTask, categories, StoreConstants.BlogsType);



                PagingHelper.StoreSettings = GetStoreSettings();
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;

                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask);

                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "BlogsController:Index:" + ex.StackTrace, page);
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
        public ActionResult Blog(String id = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.BlogsType))
                {
                    return HttpNotFound("Not Found");
                }
                int blogId = id.Split("-".ToCharArray()).Last().ToInt();
                var blogsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "BlogDetailPage");
                var contentsTask = ContentService.GetContentByIdAsync(blogId);
                var categoryTask = CategoryService.GetCategoryByContentIdAsync(StoreId, blogId);

                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("BlogsBlog_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("BlogsBlog_ImageHeight", 50);
                var dic = ContentHelper.GetContentDetailPage(contentsTask, blogsPageDesignTask, categoryTask, StoreConstants.BlogsType);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "BlogsController:Blog:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
 
    }
}