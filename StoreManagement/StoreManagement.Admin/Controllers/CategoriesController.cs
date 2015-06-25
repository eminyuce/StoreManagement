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
    [Authorize]
    public abstract class CategoriesController : BaseController
    {

        private String CategoryType { set; get; }

        public CategoriesController(String categoryType)
        {
            this.CategoryType = categoryType;
        }

        //
        // GET: /Categories/

        public ActionResult Index(int storeId = 0, String search = "")
        {
            List<Category> resultList = new List<Category>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = CategoryRepository.GetCategoriesByStoreId(storeId, CategoryType, search);
            }


            return View(resultList);
        }
        //
        // GET: /Categories/Details/5
        public ViewResult Details(int id)
        {
            Category category = CategoryRepository.GetCategory(id);
            return View(category);
        }
        //
        // GET: /Categories/Edit/5
        public ActionResult SaveOrEdit(int id = 0)
        {
            Category category = new Category();

            if (id == 0)
            {
                category.CreatedDate = DateTime.Now;
                category.UpdatedDate = DateTime.Now;
                category.State = true;
            }
            else
            {
                category = CategoryRepository.GetCategory(id);
                category.UpdatedDate = DateTime.Now;
            }
            category.CategoryType = CategoryType;

            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryType = CategoryType;
                if (category.Id == 0)
                {
                    CategoryRepository.Add(category);
                }
                else
                {
                    CategoryRepository.Edit(category);
                }
                category.CreatedDate = DateTime.Now;
                CategoryRepository.Save();


                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = category.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        //
        // GET: /Categories/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            Category category = CategoryRepository.GetSingle(id);
            return View(category);
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = CategoryRepository.GetCategory(id);
            CategoryRepository.Delete(category);
            CategoryRepository.Save();


            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = category.StoreId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult TestPage(int storeId = 1, String categoryType = "family")
        {

            ViewBag.StoreId = storeId;
            ViewBag.CategoryType = categoryType;

            return View();
        }


        public ActionResult Test()
        {
            return View();
        }

        public PartialViewResult CategoriesRadioButton(int categoryId = 0)
        {
            ViewBag.CategoryId = categoryId;
            return PartialView("_RadioButtonCategories", CategoryRepository.GetAll().ToList());
        }



    }
}