using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class CategoriesController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(String id, int page = 1)
        {
            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, "product");
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetSingle(categoryId);
            returnModel.Contents = ContentService.GetContentsCategoryId(Store.Id, categoryId, "product", true);


            return View(returnModel);

        }
    }
}