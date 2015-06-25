using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Services;

namespace StoreManagement.Controllers
{
    public class ProductCategoriesController : BaseController
    {
        

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(String id, int page = 1)
        {
            var returnModel = new ProductCategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Categories = ProductCategoryService.GetProductCategoriesByStoreId(Store.Id, StoreConstants.ProductType);
            returnModel.Store = Store;
            returnModel.Category = ProductCategoryService.GetProductCategory(categoryId);
            var m = ProductService.GetProductsCategoryId(Store.Id, categoryId, StoreConstants.ProductType, true, page, 24);
            returnModel.Products = new PagedList<Product>(m.items, m.page - 1, m.pageSize, m.totalItemCount);

            return View(returnModel);

        }
	}
}