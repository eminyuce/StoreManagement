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
        public async Task<JsonResult> MainNavigation(String desingName = "MainNavigation")
        {
            int storeId = StoreId;
            String returnHtml = "";

            try
            {
                returnHtml = await GetMainNavigation(desingName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MainNavigation:" + ex.StackTrace, storeId, desingName);

            }

            return Json(returnHtml, JsonRequestBehavior.AllowGet);


        }

        private async Task<String> GetMainNavigation(string desingName)
        {
            string returnHtml;
            var navigationsTask = NavigationService.GetStoreActiveNavigationsAsync(StoreId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

            NavigationHelper.StoreSettings = GetStoreSettings();

            await Task.WhenAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var pageOutput = NavigationHelper.GetMainLayoutLink(navigations, pageDesign);
            returnHtml = pageOutput.PageOutputText;

            return returnHtml;
        }

        public async Task<JsonResult> Footer(String desingName = "Footer")
        {
            int storeId = StoreId;

            String returnHtml = "";
            try
            {
               returnHtml = await GetMainFooter(desingName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Footer:" + ex.StackTrace, desingName, storeId);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);

        }

        private async Task<String> GetMainFooter(string desingName)
        {
            string returnHtml;
            var navigationsTask = NavigationService.GetStoreActiveNavigationsAsync(StoreId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

            await Task.WhenAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var pageOutput = NavigationHelper.GetMainLayoutFooterLink(navigations, pageDesign);
            returnHtml = pageOutput.PageOutputText;

            return returnHtml;
        }

        public async Task<JsonResult> GetComments(int itemId, String itemType, int page, String desingName = "CommentsPartial", int pageSize = 0)
        {

            pageSize = pageSize == 0 ? GetSettingValueInt("Comments_PageSize", StoreConstants.DefaultPageSize) : pageSize;

            String returnHtml = "";
            try
            {

               returnHtml = await GetCommentsHtml(itemId, itemType, page, desingName, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductComments:" + ex.StackTrace, StoreId, itemId, itemType, page, pageSize);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

        private async Task<String> GetCommentsHtml(int itemId, string itemType, int page, string desingName, int pageSize)
        {
            string returnHtml;
            var commentsTask = CommentService.GetCommentsByItemIdAsync(StoreId, itemId, itemType, page, pageSize);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

            CommentHelper.StoreSettings = GetStoreSettings();

            await Task.WhenAll(pageDesignTask, commentsTask);
            var pageDesign = pageDesignTask.Result;
            var comments = commentsTask.Result;

            var pageOuput = CommentHelper.GetCommentsPartial(comments, pageDesign);
            returnHtml = pageOuput.PageOutputText;

            return returnHtml;
        }
    }
}