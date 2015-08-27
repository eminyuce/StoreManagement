using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class NewsController : BaseController
    {
         

        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.NewsType))
                {
                    return HttpNotFound("Not Found");
                }
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
                var newsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "NewsIndex");
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, null, StoreConstants.NewsType, true, page, GetSettingValueInt("NewsIndexPageSize", StoreConstants.DefaultPageSize));
                var categories = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.NewsType, true);


                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("NewsIndex_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("NewsIndex_ImageHeight", 50);
                var pageOutput = ContentHelper.GetContentsIndexPage(contentsTask, newsPageDesignTask, categories, StoreConstants.NewsType);



                var pagingHelper = new PagingHelper();
                pagingHelper.StoreSettings = GetStoreSettings();
                pagingHelper.StoreId = StoreId;
                pagingHelper.PageOutput = pageOutput;
               
                pagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                pagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

                var pagingDic = pagingHelper.GetPaging(pagingPageDesignTask);
 
                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "NewsController:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }
        //
        // GET: /Blogs/
        public ActionResult Detail(String id = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.NewsType))
                {
                    return HttpNotFound("Not Found");
                }
                int newsId = id.Split("-".ToCharArray()).Last().ToInt();
                var blogsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "NewsDetailPage");
                var contentsTask = ContentService.GetContentByIdAsync(newsId);
                var categoryTask = CategoryService.GetCategoryByContentIdAsync(StoreId, newsId);

                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("NewsDetail_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("NewsDetail_ImageHeight", 50);
                var dic = ContentHelper.GetContentDetailPage(contentsTask, blogsPageDesignTask, categoryTask, StoreConstants.NewsType);


                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "NewsController:News:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }

        
    }
}