using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class AjaxGenericsController : BaseController
    {
        public async Task<JsonResult> MainLayout()
        {
            int storeId = StoreId;

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainNavigation");

                    NavigationHelper.StoreSettings = GetStoreSettings();
                    var pageOutput = NavigationHelper.GetMainLayoutLink(mainMenu, pageDesignTask);
                    String html = pageOutput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "MainLayout", storeId);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);


        }
        public async Task<JsonResult> MainLayoutFooter()
        {
            int storeId = StoreId;

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "Footer");
                    var pageOutput = NavigationHelper.GetMainLayoutFooterLink(mainMenu, pageDesignTask);
                    String html = pageOutput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "MainLayout", storeId);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> GetComments(int itemId, String itemType, int page, String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("Comments_DefaultPageDesign", "CommentsPartial");
            }
            int pageSize = GetSettingValueInt("Comments_PageSize", StoreConstants.DefaultPageSize);

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
      
                    var categoriesTask = CommentService.GetCommentsByItemIdAsync(StoreId, itemId, itemType, page, pageSize);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                    CommentHelper.StoreSettings = GetStoreSettings();
                    var pageOuput = CommentHelper.GetCommentsPartial(categoriesTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "ProductComments");
                    return "";
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
    }
}