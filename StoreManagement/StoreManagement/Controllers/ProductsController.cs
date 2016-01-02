using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Services;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class ProductsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            var returnModel = new ProductsViewModel();
            returnModel.Categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(MyStore.Id, StoreConstants.ProductType);
            returnModel.Store = MyStore;
            return View(returnModel);
        }
        //
        // GET: /Products/
        public ActionResult Product(String id)
        {
            var returnModel = new ProductDetailViewModel();
            int productId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Product = ProductService.GetProductsById(productId);
            returnModel.Store = MyStore;
            returnModel.Category = ProductCategoryService.GetProductCategory(returnModel.Product.ProductCategoryId);
            returnModel.Categories = ProductCategoryService.GetProductCategoriesByStoreId(MyStore.Id, StoreConstants.ProductType);
            return View(returnModel);
        }
	}
}