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
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class NewsController : BaseController
    {

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index(int page = 1, String search = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.NewsType))
                {
                    return HttpNotFound("Not Found");
                }
 
                var newsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "NewsIndex");
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, null, StoreConstants.NewsType, true, page, GetSettingValueInt("NewsIndexPageSize", StoreConstants.DefaultPageSize),search);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.NewsType, true);


                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("NewsIndex_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("NewsIndex_ImageHeight", 50);

                await Task.WhenAll(newsPageDesignTask, contentsTask, categoriesTask);
                var contents = contentsTask.Result;
                var pageDesign = newsPageDesignTask.Result;
                var categories = categoriesTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var pageOutput = ContentHelper.GetContentsIndexPage(contents, pageDesign, categories, StoreConstants.NewsType);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");



                PagingHelper.StoreSettings = GetStoreSettings();
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask.Result);
 
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
        public async Task<ActionResult> Detail(String id = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.NewsType))
                {
                    return HttpNotFound("Not Found");
                }
                int newsId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "NewsDetailPage");
                var contentTask = ContentService.GetContentByIdAsync(newsId);
                var categoryTask = CategoryService.GetCategoryByContentIdAsync(StoreId, newsId);


                ContentHelper.StoreId = this.StoreId;
                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("NewsDetail_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("NewsDetail_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, contentTask, categoryTask);
                var content = contentTask.Result;
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;

                var dic = ContentHelper.GetContentDetailPage(content, pageDesign, category, StoreConstants.NewsType);


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