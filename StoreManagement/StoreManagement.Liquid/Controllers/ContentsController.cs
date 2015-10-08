using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class ContentsController : BaseController
    {
        protected String PageDesingIndexPageName { get; set; }
        protected String PageDesingDetailPageName { get; set; }

        private String Type { get; set; }
        protected ContentsController(String type)
        {
            this.Type = type;
        }

        public virtual async Task<ActionResult> Index(int page = 1, String search = "", int ? categoryId = null)
        {
            try
            {
                if (!IsModulActive(Type))
                {
                    return HttpNotFound("Not Found");
                }

                var newsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingIndexPageName);
                var pageSize = GetSettingValueInt(Type+"IndexPageSize", StoreConstants.DefaultPageSize);
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, categoryId, Type, true, page, pageSize, search);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, Type, true);


                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt(Type + "Index_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt(Type + "Index_ImageHeight", 50);

                await Task.WhenAll(newsPageDesignTask, contentsTask, categoriesTask);
                var contents = contentsTask.Result;
                var pageDesign = newsPageDesignTask.Result;
                var categories = categoriesTask.Result;

                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingIndexPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }


                var pageOutput = ContentHelper.GetContentsIndexPage(contents, pageDesign, categories, Type);
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
                Logger.Error(ex, Type+"Controller:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }
        //
        // GET: /Blogs/
        public virtual async Task<ActionResult> Detail(String id = "")
        {
            try
            {
                if (!IsModulActive(Type))
                {
                    return HttpNotFound("Not Found");
                }
                int newsId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingDetailPageName);
                var contentTask = ContentService.GetContentByIdAsync(newsId);
                var categoryTask = CategoryService.GetCategoryByContentIdAsync(StoreId, newsId);


                ContentHelper.StoreId = this.StoreId;
                ContentHelper.StoreSettings = GetStoreSettings();
                ContentHelper.ImageWidth = GetSettingValueInt(Type + "Detail_ImageWidth", 50);
                ContentHelper.ImageHeight = GetSettingValueInt(Type + "Detail_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, contentTask, categoryTask);
                var content = contentTask.Result;
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + PageDesingDetailPageName);
                }


                var dic = ContentHelper.GetContentDetailPage(content, pageDesign, category, Type);


                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, Type + "Controller" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}