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

                var newsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "NewsIndex");
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, null, StoreConstants.NewsType, true, page, GetSettingValueInt("NewsIndexPageSize", StoreConstants.DefaultPageSize));
                var categories = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.NewsType, true);
                var liquidHelper = new ContentHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                liquidHelper.ImageWidth = GetSettingValueInt("NewsIndex_ImageWidth", 50);
                liquidHelper.ImageHeight = GetSettingValueInt("NewsIndex_ImageHeight", 50);

                var dic = liquidHelper.GetContentsIndexPage(contentsTask, newsPageDesignTask, categories, StoreConstants.NewsType);

                return View(dic);

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
                var liquidHelper = new ContentHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                var dic = liquidHelper.GetContentDetailPage(contentsTask, blogsPageDesignTask, categoryTask, StoreConstants.NewsType);
                liquidHelper.ImageWidth = GetSettingValueInt("NewsDetail_ImageWidth", 50);
                liquidHelper.ImageHeight = GetSettingValueInt("NewsDetail_ImageHeight", 50);

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