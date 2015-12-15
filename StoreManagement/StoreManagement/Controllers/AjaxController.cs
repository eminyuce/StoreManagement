using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    public class AjaxController : BaseController
    {


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRelatedContents(int categoryId, String contentType)
        {
            var returnModel = new ContentDetailViewModel();
            returnModel.Store = MyStore;
            returnModel.Category = CategoryService.GetCategory(categoryId);
            returnModel.RelatedContents = ContentService.GetContentByTypeAndCategoryId(MyStore.Id, contentType, categoryId, "", true).Take(5).ToList();
            String partialViewName = @"pContents\pRelatedContents";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelatedProducts(int categoryId)
        {
            var returnModel = new ProductDetailViewModel();
            returnModel.Store = MyStore;
            returnModel.Category = ProductCategoryService.GetProductCategory(categoryId);
            returnModel.RelatedProducts = ProductService.GetProductByTypeAndCategoryId(MyStore.Id, "product", categoryId).Take(5).ToList();
            String partialViewName = @"pProducts\pRelatedProducts";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductCategories()
        {
            var categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(MyStore.Id, StoreConstants.ProductType);
            String partialViewName = @"pProducts\pProductCategories";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(categories));
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Refresh(String domain)
        {

            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}