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
        public ActionResult Category(String id, int page = 7)
        {
            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, "product");
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetSingle(categoryId);
            var m = ContentService.GetContentsCategoryId(Store.Id, categoryId, "product", true, page, 25);
            returnModel.Contents = new PagedList<Content>(m.items, m.page, m.pageSize, m.totalItemCount);


            return View(returnModel);

        }
    }
}