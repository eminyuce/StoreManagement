using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class CategoriesController : BaseController
    {

        private ICategoryRepository categoryRepository;

        public CategoriesController(IStoreContext dbContext,
            ISettingRepository settingRepository,
            ICategoryRepository categoryRepository)
            : base(dbContext, settingRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        //
        // GET: /Categories/

        public ActionResult Index(int storeId = 0, String search = "")
        {
            List<Category> resultList = new List<Category>();
            if (storeId == 0)
            {
                resultList = categoryRepository.GetAll().ToList();
            }
            else
            {
                resultList = categoryRepository.GetCategoriesByStoreId(storeId);
            }

            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            return View(resultList);
        }
        //
        // GET: /Categories/Details/5
        public ViewResult Details(int id)
        {
            Category category = categoryRepository.GetSingle(id);
            return View(category);
        }
        //
        // GET: /Categories/Edit/5
        public ActionResult SaveOrEdit(int id = 0)
        {
            Category category = new Category();

            if (id == 0)
            {

            }
            else
            {
                category = categoryRepository.GetSingle(id);
            }


            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    categoryRepository.Add(category);
                }
                else
                {
                    categoryRepository.Edit(category);
                }
                categoryRepository.Save();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Categories/Delete/5

        public ActionResult Delete(int id)
        {
            Category category = categoryRepository.GetSingle(id);
            return View(category);
        }
       
        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryRepository.GetSingle(id);
            categoryRepository.Delete(category);
            categoryRepository.Save();
            return RedirectToAction("Index");
        }

       

        public ActionResult TestPage(int storeId = 1, String categoryType = "family")
        {
            ViewBag.StoreId = storeId;
            ViewBag.CategoryType = categoryType;

            return View();
        }

        public ActionResult CreateCategoryTree(int storeId = 1, String categoryType = "family")
        {
            var tree = this.categoryRepository.GetCategoriesByStoreId(storeId, categoryType);
            return View(tree);
        }
        public ActionResult Test()
        {
            return View();
        }

        public PartialViewResult CategoriesRadioButton(int categoryId = 0)
        {
            ViewBag.CategoryId = categoryId;
            return PartialView("_RadioButtonCategories", categoryRepository.GetAll().ToList());
        }



    }
}