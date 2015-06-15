using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRelatedContents(int categoryId)
        {
            var returnModel = new ContentDetailViewModel();
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetCategory(categoryId);
            returnModel.RelatedContents =ContentService.GetContentByTypeAndCategoryId(Store.Id, "product", categoryId).Take(5).ToList();
            String partialViewName = "pRelatedContents";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRelatedProducts(int categoryId)
        {
            var returnModel = new ProductDetailViewModel();
            returnModel.Store = Store;
            returnModel.Category = ProductCategoryService.GetProductCategory(categoryId);
            returnModel.RelatedProducts =ProductService.GetProductByTypeAndCategoryId(Store.Id, "product", categoryId).Take(5).ToList();
            String partialViewName = "pRelatedContents";
            var html = this.RenderPartialToString(partialViewName, new ViewDataDictionary(returnModel));
            return Json(html, JsonRequestBehavior.AllowGet);
        }
	}
}