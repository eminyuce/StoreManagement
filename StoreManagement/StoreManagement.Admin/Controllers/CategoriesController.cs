using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class CategoriesController : BaseController
    {

        private ICategoryRepository categoryRepository;
        public CategoriesController(IStoreContext dbContext, ICategoryRepository categoryRepository) : base(dbContext)
        {
            this.categoryRepository = categoryRepository;
        }
        public ActionResult Index(int storeId = 1, String categoryType = "family")
        {
            ViewBag.StoreId = storeId;
            ViewBag.CategoryType = categoryType;

            return View();
        }
        
        public ActionResult CreateCategoryTree(int storeId=1, String categoryType="family")
        {
            var tree = this.categoryRepository.GetCategoriesByStoreId(storeId, categoryType);
            return View(tree);
        }
        public ActionResult Test()
        {
            return View();
        }
        
    }
}