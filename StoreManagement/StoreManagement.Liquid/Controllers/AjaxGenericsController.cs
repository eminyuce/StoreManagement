using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Attributes;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    [Compress]
    public class AjaxGenericsController : AjaxController
    {
        public async Task<JsonResult> MainNavigation(String designName = "MainNavigationPartial")
        {
            int storeId = StoreId;
            String returnHtml = "";

            try
            {
                returnHtml = await GetMainNavigation(designName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "MainNavigationPartial:" + ex.StackTrace, storeId, designName);

            }

            return Json(returnHtml, JsonRequestBehavior.AllowGet);


        }

        private async Task<String> GetMainNavigation(string designName)
        {
            string returnHtml;
            var navigationsTask = NavigationService.GetStoreActiveNavigationsAsync(StoreId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);

            NavigationHelper.StoreSettings = GetStoreSettings();

            await Task.WhenAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var pageOutput = NavigationHelper.GetMainLayoutLink(navigations, pageDesign);
            returnHtml = pageOutput.PageOutputText;

            return returnHtml;
        }

        public async Task<JsonResult> Footer(String designName = "FooterPartial")
        {
            int storeId = StoreId;

            String returnHtml = "";
            try
            {
                returnHtml = await GetMainFooter(designName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "FooterPartial:" + ex.StackTrace, designName, storeId);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);

        }

        private async Task<String> GetMainFooter(string designName)
        {
            string returnHtml;
            var navigationsTask = NavigationService.GetStoreActiveNavigationsAsync(StoreId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);

            await Task.WhenAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var pageOutput = NavigationHelper.GetMainLayoutFooterLink(navigations, pageDesign);
            returnHtml = pageOutput.PageOutputText;

            return returnHtml;
        }

        public async Task<JsonResult> GetComments(int itemId, String itemType, int page, String designName = "CommentsPartial", int pageSize = 0)
        {

            pageSize = pageSize == 0 ? GetSettingValueInt("Comments_PageSize", StoreConstants.DefaultPageSize) : pageSize;

            String returnHtml = "";
            try
            {

                returnHtml = await GetCommentsHtml(itemId, itemType, page, designName, pageSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductComments:" + ex.StackTrace, StoreId, itemId, itemType, page, pageSize);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

        private async Task<String> GetCommentsHtml(int itemId, string itemType, int page, string designName, int pageSize)
        {
            string returnHtml;
            var commentsTask = CommentService.GetCommentsByItemIdAsync(StoreId, itemId, itemType, page, pageSize);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);

            CommentHelper.StoreSettings = GetStoreSettings();

            await Task.WhenAll(pageDesignTask, commentsTask);
            var pageDesign = pageDesignTask.Result;
            var comments = commentsTask.Result;

            var pageOuput = CommentHelper.GetCommentsPartial(comments, pageDesign);
            returnHtml = pageOuput.PageOutputText;

            return returnHtml;
        }
        public ActionResult SearchAutoComplete(String term, String type, int take=0)
        {

            var list = new List<String>();
            String searchKey = term;

            take = take == 0 ? 10 : take;

            if (type.Equals("products", StringComparison.InvariantCultureIgnoreCase))
            {
                list =
                    ProductService.GetProductByTypeAndCategoryId(StoreId, StoreConstants.ProductType, 0, searchKey, true).Take(take)
                                  .Select(r => r.Name)
                                  .ToList();
            }
            else if (type.Equals(StoreConstants.BlogsType, StringComparison.InvariantCultureIgnoreCase) || type.Equals(StoreConstants.NewsType, StringComparison.InvariantCultureIgnoreCase))
            {

                list =
                       ContentService.GetContentByTypeAndCategoryId(StoreId, type, 0, searchKey, true).Take(take)
                                     .Select(r => r.Name)
                                     .ToList();
            }
           


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetContactForm(String designName = "ContactFormPartial")
        {


            String returnHtml = "";
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);

                CommentHelper.StoreSettings = GetStoreSettings();

                await Task.WhenAll(pageDesignTask);
                var pageDesign = pageDesignTask.Result;

                returnHtml = pageDesign.PageTemplate;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ContactFormPartial:" + ex.StackTrace, StoreId, designName);

            }


            return Json(returnHtml, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult SaveContactForm(Message message)
        {
            //MessageService.SaveContactForm(message);
            Logger.Trace("Message "+message);
            message.CreatedDate = DateTime.Now;
            message.UpdatedDate = DateTime.Now;
            message.State = true;
            message.StoreId = StoreId;
            message.Ordering = 1;
            MessageService.SaveContactFormMessage(message);
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}