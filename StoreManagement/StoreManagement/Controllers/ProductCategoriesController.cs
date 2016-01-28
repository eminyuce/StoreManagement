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


namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class ProductCategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(String id, int page = 1)
        {
            var resultModel = new ProductCategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
            resultModel.SCategories = ProductCategoryService.GetProductCategoriesByStoreId(MyStore.Id, StoreConstants.ProductType);
            resultModel.SStore = MyStore;
            resultModel.SCategory = ProductCategoryService.GetProductCategory(categoryId);
            var m = ProductService.GetProductsCategoryId(MyStore.Id, categoryId, StoreConstants.ProductType, true, page, 24);
            resultModel.SProducts = new PagedList<Product>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            resultModel.SNavigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();
            return View(resultModel);

        }
	}
}