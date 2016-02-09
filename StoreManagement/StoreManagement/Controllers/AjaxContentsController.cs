using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Attributes;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    [Compress]
    public class AjaxContentsController : AjaxController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //
        // GET: /AjaxContents/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRelatedContents(int categoryId, String contentType)
        {
            var returnModel = new ContentDetailViewModel();
            returnModel.SStore = MyStore;
            returnModel.SCategory = CategoryService.GetCategory(categoryId);
            returnModel.SRelatedContents = ContentService.GetContentByTypeAndCategoryId(MyStore.Id, contentType, categoryId, "", true).Take(5).ToList();
            String partialViewName = @"pContents\pRelatedContents";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType, int excludedContentId = 0, String designName = "", int take = 0, int imageWidth = 0, int imageHeight = 0)
        {

            if (String.IsNullOrEmpty(designName))
            {
                return Json("No Desing Name is defined.", JsonRequestBehavior.AllowGet);
            }

            String returnHtml = "";

            try
            {
                returnHtml = await GetRelatedContentsHtml(categoryId, contentType, excludedContentId, designName, take, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedContents:" + ex.StackTrace, StoreId, contentType, categoryId, take, excludedContentId);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

        private async Task<String> GetRelatedContentsHtml(int categoryId, string contentType, int excludedContentId, string designName, int take,
            int imageWidth, int imageHeight)
        {
            string returnHtml;
            var categoryTask = CategoryService.GetCategoryAsync(categoryId);


            if (take == 0 && contentType.Equals(StoreConstants.NewsType))
            {
                take = GetSettingValueInt("RelatedNews_ItemsNumber", 5);
            }
            else if (take == 0 && contentType.Equals(StoreConstants.BlogsType))
            {
                take = GetSettingValueInt("RelatedBlogs_ItemsNumber", 5);
            }

            var relatedContentsTask = ContentService.GetContentByTypeAndCategoryIdAsync(StoreId, contentType, categoryId, take,
                excludedContentId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);




            if (contentType.Equals(StoreConstants.NewsType))
            {
                ContentService2.ImageWidth = imageWidth;
                ContentService2.ImageHeight = imageHeight;
            }
            else if (contentType.Equals(StoreConstants.BlogsType))
            {
                ContentService2.ImageWidth = imageWidth;
                ContentService2.ImageHeight = imageHeight;
            }
            else
            {
                ContentService2.ImageWidth = 0;
                ContentService2.ImageHeight = 0;
                Logger.Trace("No ContentType is defined like that " + contentType);
            }

            await Task.WhenAll(pageDesignTask, relatedContentsTask, categoryTask);
            var contents = relatedContentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;

            var pageOutput = ContentService2.GetRelatedContentsPartial(category, contents, pageDesign, contentType);
            returnHtml = pageOutput.PageOutputText;

            return returnHtml;
        }


        public async Task<JsonResult> GetContentsByContentType(int page = 1, String designName = "ContentsByContentTypePartial", int categoryId = 0,
            int pageSize = 0, int imageWidth = 0, int imageHeight = 0, String type = StoreConstants.BlogsType, String contentType = "popular", int excludedContentId = 0)
        {

            if (String.IsNullOrEmpty(designName))
            {
                return Json("No Desing Name is defined.", JsonRequestBehavior.AllowGet);
            }
            String returnHtml = "";

            try
            {

                returnHtml = await GetContentsByContentTypeHtml(page, pageSize, designName, categoryId, imageWidth, imageHeight, type, contentType);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ContentsByContentTypePartial:" + ex.StackTrace, StoreId, categoryId, type, page, pageSize, contentType);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

        private async Task<String> GetContentsByContentTypeHtml(int page, int pageSize, string designName, int categoryId, int imageWidth, int imageHeight,
            string type, string contentType)
        {

            string returnHtml;
            var catId = categoryId == 0 ? (int?)null : categoryId;
            ContentService2.ImageWidth = imageWidth;
            ContentService2.ImageHeight = imageHeight;
            Task<List<Content>> contentsTask = ContentService.GetContentsByContentKeywordAsync(StoreId, catId, type, page,
                pageSize, true, contentType);


            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, type, true);

            await Task.WhenAll(pageDesignTask, contentsTask, categoriesTask);
            var contents = contentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;

            var pageOuput = ContentService2.GetContentsByContentType(contents, categories, pageDesign, type);
            returnHtml = pageOuput.PageOutputText;

            return returnHtml;
        }

    }
}