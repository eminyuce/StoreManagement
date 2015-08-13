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
    public class AjaxContentsController : BaseController
    {


        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType, int excludedContentId = 0, String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedContentsPartial";
            }


            var res = Task.Factory.StartNew(() =>
            {
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
                    var liquidHelper = new ContentHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();



                    if (contentType.Equals(StoreConstants.NewsType))
                    {
                        liquidHelper.ImageWidth = GetSettingValueInt("RelatedNewsPartial_ImageWidth", 50);
                        liquidHelper.ImageHeight = GetSettingValueInt("RelatedNewsPartial_ImageHeight", 50);
                    }
                    else if (contentType.Equals(StoreConstants.BlogsType))
                    {
                        liquidHelper.ImageWidth = GetSettingValueInt("RelatedBlogsPartial_ImageWidth", 50);
                        liquidHelper.ImageHeight = GetSettingValueInt("RelatedBlogsPartial_ImageHeight", 50);
                    }
                    else
                    {
                        liquidHelper.ImageWidth = 0;
                        liquidHelper.ImageHeight = 0;
                        Logger.Trace("No ContentType is defined like that " + contentType);
                    }


                    Dictionary<String, String> dic = liquidHelper.GetRelatedContentsPartial(categoryTask, relatedContentsTask, pageDesignTask, contentType);
                    String html = dic[StoreConstants.PageOutput];
                    return html;

                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRelatedContents", contentType, categoryId);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }


     
   


    }
}