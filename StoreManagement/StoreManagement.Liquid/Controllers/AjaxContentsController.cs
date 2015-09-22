using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class AjaxContentsController : BaseController
    {
        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType, int excludedContentId = 0, String designName = "", int take = 0, int imageWidth = 0, int imageHeight = 0)
        {

            if (String.IsNullOrEmpty(designName))
            {
                return Json("No Desing Name is defined.", JsonRequestBehavior.AllowGet);
            }

            String returtHtml = "";

            try
            {
                var categoryTask = CategoryService.GetCategoryAsync(categoryId);


                if (take == 0 && contentType.Equals(StoreConstants.NewsType))
                {
                    take = GetSettingValueInt("RelatedNews_ItemsNumber", 5);
                }
                else if (take == 0 && contentType.Equals(StoreConstants.BlogsType))
                {
                    take = GetSettingValueInt("RelatedBlogs_ItemsNumber", 5);
                }

                var relatedContentsTask = ContentService.GetContentByTypeAndCategoryIdAsync(StoreId, contentType, categoryId, take, excludedContentId);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);

                ContentHelper.StoreSettings = GetStoreSettings();



                if (contentType.Equals(StoreConstants.NewsType))
                {
                    ContentHelper.ImageWidth = imageWidth == 0 ? GetSettingValueInt("RelatedNewsPartial_ImageWidth", 50) : imageWidth;
                    ContentHelper.ImageHeight = imageHeight == 0 ? GetSettingValueInt("RelatedNewsPartial_ImageHeight", 50) : imageHeight;
                }
                else if (contentType.Equals(StoreConstants.BlogsType))
                {
                    ContentHelper.ImageWidth = imageWidth == 0 ? GetSettingValueInt("RelatedBlogsPartial_ImageWidth", 50) : imageWidth;
                    ContentHelper.ImageHeight = imageHeight == 0 ? GetSettingValueInt("RelatedBlogsPartial_ImageHeight", 50) : imageHeight;
                }
                else
                {
                    ContentHelper.ImageWidth = 0;
                    ContentHelper.ImageHeight = 0;
                    Logger.Trace("No ContentType is defined like that " + contentType);
                }

                await Task.WhenAll(pageDesignTask, relatedContentsTask, categoryTask);
                var contents = relatedContentsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;

                var pageOutput = ContentHelper.GetRelatedContentsPartial(category, contents, pageDesign, contentType);
                returtHtml = pageOutput.PageOutputText;


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedContents", contentType, categoryId);

            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetContentsByContentType(int page = 1, String desingName = "", int categoryId = 0,
            int pageSize = 0, int imageWidth = 0, int imageHeight = 0, String type = StoreConstants.BlogsType, String contentType = "popular")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("ContentsByContentType_DefaultPageDesign", "ContentsByContentType");
            }
            String returtHtml = "";

            try
            {

                 var catId = categoryId == 0 ? (int?)null : categoryId;
               
                if (contentType.Equals("popular"))
                {
                    pageSize = pageSize == 0 ? GetSettingValueInt("PopularContents_PageSize", StoreConstants.DefaultPageSize) : pageSize;
                    ContentHelper.ImageWidth = imageWidth == 0 ? GetSettingValueInt("PopularContents_ImageWidth", 99) : imageWidth;
                    ContentHelper.ImageHeight = imageHeight == 0 ? GetSettingValueInt("PopularContents_ImageHeight", 99) : imageHeight;

                }
                else if (contentType.Equals("recent"))
                {
                    pageSize = pageSize == 0 ? GetSettingValueInt("RecentContents_PageSize", StoreConstants.DefaultPageSize) : pageSize;
                    ContentHelper.ImageWidth = imageWidth == 0 ? GetSettingValueInt("RecentContents_ImageWidth", 99) : imageWidth;
                    ContentHelper.ImageHeight = imageHeight == 0 ? GetSettingValueInt("RecentContents_ImageHeight", 99) : imageHeight;
                }
                else if (contentType.Equals("main"))
                {
                    pageSize = pageSize == 0 ? GetSettingValueInt("MainContents_PageSize", StoreConstants.DefaultPageSize) : pageSize;
                    ContentHelper.ImageWidth = imageWidth == 0 ? GetSettingValueInt("MainContents_ImageWidth", 99) : imageWidth;
                    ContentHelper.ImageHeight = imageHeight == 0 ? GetSettingValueInt("MainContents_ImageHeight", 99) : imageHeight;
                }
                Task<List<Content>> contentsTask = ContentService.GetContentsByContentKeywordAsync(StoreId, catId, type, page, pageSize, true, contentType);


                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, type, true);

                await Task.WhenAll(pageDesignTask, contentsTask, categoriesTask);
                var contents = contentsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;

                ContentHelper.StoreSettings = GetStoreSettings();


                var pageOuput = ContentHelper.GetContentsByContentType(contents, categories, pageDesign, type);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContentsByContentType");

            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }



    }
}