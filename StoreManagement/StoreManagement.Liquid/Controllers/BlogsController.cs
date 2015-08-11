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


                var liquidHelper = new ContentHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                liquidHelper.ImageWidth = GetSettingValueInt("BlogsIndex_ImageWidth", 50);
                liquidHelper.ImageHeight = GetSettingValueInt("BlogsIndex_ImageHeight", 50);
                var dic = liquidHelper.GetContentsIndexPage(contentsTask, blogsPageDesignTask, categories, StoreConstants.BlogsType);


                var pagingHelper = new PagingHelper();
                pagingHelper.StoreSettings = GetStoreSettings();
                pagingHelper.StoreId = StoreId;
                pagingHelper.PageOutputDictionary = dic;

                pagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                pagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var pagingDic = pagingHelper.GetPaging(pagingPageDesignTask);

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
                var liquidHelper = new ContentHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                liquidHelper.ImageWidth = GetSettingValueInt("BlogsBlog_ImageWidth", 50);
                liquidHelper.ImageHeight = GetSettingValueInt("BlogsBlog_ImageHeight", 50);
                var dic = liquidHelper.GetContentDetailPage(contentsTask, blogsPageDesignTask, categoryTask, StoreConstants.BlogsType);

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