using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using Ninject;
using StoreManagement.Data.Entities;
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
            returnModel.Category = CategoryService.GetCategory(categoryId);
            var m = ContentService.GetContentsCategoryId(Store.Id, categoryId, "product", true, page, 24);
            returnModel.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);


            return View(returnModel);

        }
    }
}