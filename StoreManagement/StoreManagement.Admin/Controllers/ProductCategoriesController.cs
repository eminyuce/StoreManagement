using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Repositories;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class ProductCategoriesController : BaseController
    {
        //
        // GET: /Categories/
        public ActionResult Index(int storeId = 0, String search = "")
        {
            List<ProductCategory> resultList = new List<ProductCategory>();
            storeId = GetStoreId(storeId);
            if (storeId == 0)
            {
                resultList = ProductCategoryRepository.GetAll().ToList();
            }
            else
            {
                resultList = ProductCategoryRepository.GetProductCategoriesByStoreId(storeId);
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
            ProductCategory category = ProductCategoryRepository.GetProductCategory(id);
            return View(category);
        }

        //
        // GET: /Categories/Edit/5
        public ActionResult SaveOrEdit(int id = 0)
        {
            ProductCategory category = new ProductCategory();

            if (id == 0)
            {

            }
            else
            {
                category = ProductCategoryRepository.GetProductCategory(id);
            }


            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    ProductCategoryRepository.Add(category);
                }
                else
                {
                    ProductCategoryRepository.Edit(category);
                }
                category.CreatedDate = DateTime.Now;
                ProductCategoryRepository.Save();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Categories/Delete/5

        public ActionResult Delete(int id)
        {
            ProductCategory category = ProductCategoryRepository.GetSingle(id);
            return View(category);
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory category = ProductCategoryRepository.GetProductCategory(id);
            ProductCategoryRepository.Delete(category);
            ProductCategoryRepository.Save();
            return RedirectToAction("Index");
        }

    }
}