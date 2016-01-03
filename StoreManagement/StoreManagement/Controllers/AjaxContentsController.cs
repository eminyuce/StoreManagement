using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    public class AjaxContentsController : AjaxController
    {
        //
        // GET: /AjaxContents/
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
     
	}
}