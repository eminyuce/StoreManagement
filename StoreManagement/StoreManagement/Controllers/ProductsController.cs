using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services;

namespace StoreManagement.Controllers
{
    public class ProductsController : BaseController
    {

        public ActionResult Index()
        {
            var returnModel = new ProductsViewModel();
            returnModel.Categories = CategoryService.GetCategoriesByStoreIdFromCache(Store.Id, "product");
            returnModel.Store = Store;
            return View(returnModel);
        }
        //
        // GET: /Products/
        public ActionResult Product(String id)
        {
            var returnModel = new ProductDetailViewModel();
            int productId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(productId);
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetSingle(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, "product");
            return View(returnModel);
        }
	}
}