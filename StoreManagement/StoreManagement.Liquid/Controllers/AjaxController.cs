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
    public class AjaxController : BaseController
    {
         

        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType)
        {
            var res = Task.Factory.StartNew(() =>
            {

                var categoryTask = CategoryService.GetCategoryAsync(categoryId);
                int take = GetSettingValueInt("RelatedContents_ItemsNumber", 5);
                var relatedContentsTask = ContentService.GetContentByTypeAndCategoryIdAsync(StoreId, contentType, categoryId, take);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "RelatedContentsPartial");
                var liquidHelper = new ContentHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                Dictionary<String, String> dic = liquidHelper.GetRelatedContentsPartial(categoryTask, relatedContentsTask, pageDesignTask, contentType);
                String html = dic["PageOutput"];
                return html;

            });

            return Json(await res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelatedProducts(int categoryId)
        {
            var returnModel = new ProductDetailViewModel();
            // returnModel.Store = Store;
            returnModel.Category = ProductCategoryService.GetProductCategory(categoryId);
            returnModel.RelatedProducts = ProductService.GetProductByTypeAndCategoryId(StoreId, "product", categoryId).Take(5).ToList();
            String partialViewName = @"pProducts\pRelatedProducts";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductCategories()
        {
            var categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(StoreId, StoreConstants.ProductType);
            String partialViewName = @"pProducts\pProductCategories";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(categories));
            return Json(html, JsonRequestBehavior.AllowGet);
        }

       
    }
}