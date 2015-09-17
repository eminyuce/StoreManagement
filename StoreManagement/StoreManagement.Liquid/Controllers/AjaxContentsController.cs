using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class AjaxContentsController : BaseController
    {
        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType, int excludedContentId = 0, String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedContentsPartial";
            }

            String returtHtml = "";

            try
            {
                var categoryTask = CategoryService.GetCategoryAsync(categoryId);

                int take = 0;
                if (contentType.Equals(StoreConstants.NewsType))
                {
                    take = GetSettingValueInt("RelatedNews_ItemsNumber", 5);
                }
                else if (contentType.Equals(StoreConstants.BlogsType))
                {
                    take = GetSettingValueInt("RelatedBlogs_ItemsNumber", 5);
                }

                var relatedContentsTask = ContentService.GetContentByTypeAndCategoryIdAsync(StoreId, contentType, categoryId, take, excludedContentId);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                ContentHelper.StoreSettings = GetStoreSettings();



                if (contentType.Equals(StoreConstants.NewsType))
                {
                    ContentHelper.ImageWidth = GetSettingValueInt("RelatedNewsPartial_ImageWidth", 50);
                    ContentHelper.ImageHeight = GetSettingValueInt("RelatedNewsPartial_ImageHeight", 50);
                }
                else if (contentType.Equals(StoreConstants.BlogsType))
                {
                    ContentHelper.ImageWidth = GetSettingValueInt("RelatedBlogsPartial_ImageWidth", 50);
                    ContentHelper.ImageHeight = GetSettingValueInt("RelatedBlogsPartial_ImageHeight", 50);
                }
                else
                {
                    ContentHelper.ImageWidth = 0;
                    ContentHelper.ImageHeight = 0;
                    Logger.Trace("No ContentType is defined like that " + contentType);
                }

                await  Task.WhenAll(pageDesignTask, relatedContentsTask, categoryTask);
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






    }
}