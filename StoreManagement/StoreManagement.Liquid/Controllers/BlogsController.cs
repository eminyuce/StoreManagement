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

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index(int page = 1, String search = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.BlogsType))
                {
                    return HttpNotFound("Not Found");
                }
               // String search = id;
                int ? categoryId = null;

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "BlogsIndex");
                var pageSize = GetSettingValueInt("BlogsIndex_PageSize", StoreConstants.DefaultPageSize);
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, categoryId, StoreConstants.BlogsType, true, page, pageSize, search);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.BlogsType, true);



                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("BlogsIndex_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("BlogsIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, contentsTask, categoriesTask);
                var contents = contentsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }

                var pageOutput = ContentHelper.GetContentsIndexPage(contents, pageDesign, categories, StoreConstants.BlogsType);
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

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Blog(String id = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.BlogsType))
                {
                    return HttpNotFound("Not Found");
                }
                int blogId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "BlogDetailPage");
                var contentTask = ContentService.GetContentByIdAsync(blogId);
                var categoryTask = CategoryService.GetCategoryByContentIdAsync(StoreId, blogId);

                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt("BlogsBlog_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt("BlogsBlog_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, contentTask, categoryTask);
                var content = contentTask.Result;
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;

                var dic = ContentHelper.GetContentDetailPage(content, pageDesign, category, StoreConstants.BlogsType);

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