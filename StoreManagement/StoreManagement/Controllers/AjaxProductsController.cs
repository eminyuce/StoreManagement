using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotLiquid.FileSystems;
using NLog;
using StoreManagement.Data.Attributes;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{ 

    [OutputCache(CacheProfile = "Cache1Days")]
    [Compress]
    public class AjaxProductsController : AjaxController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //
        // GET: /AjaxProducts/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRelatedProducts(int categoryId)
        {
            var returnModel = new ProductDetailViewModel();
            returnModel.Store = MyStore;
            returnModel.Category = ProductCategoryService.GetProductCategory(categoryId);
            returnModel.RelatedProducts = ProductService.GetProductByTypeAndCategoryId(MyStore.Id, StoreConstants.ProductType, categoryId).Take(5).ToList();
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

	}
}