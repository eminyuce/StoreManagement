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
            returnModel.Categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(Store.Id, "product");
            returnModel.Store = Store;
            return View(returnModel);
        }
        //
        // GET: /Products/
        public ActionResult Product(String id)
        {
            var returnModel = new ProductDetailViewModel();
            int productId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Product = ProductService.GetProductsProductId(productId);
            returnModel.Store = Store;
            returnModel.Category = ProductCategoryService.GetProductCategory(returnModel.Product.ProductCategoryId);
            returnModel.Categories = ProductCategoryService.GetProductCategoriesByStoreId(Store.Id, "product");
            return View(returnModel);
        }
	}
}