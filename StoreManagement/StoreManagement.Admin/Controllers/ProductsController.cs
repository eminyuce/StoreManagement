using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private const String ProductType = "product";


        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            List<Product> resultList = new List<Product>();
            storeId = GetStoreId(storeId);
            if (storeId == 0)
            {
                resultList = ProductRepository.GetProductByType(ProductType);
            }
            else
            {
                resultList = ProductRepository.GetProductByType(storeId, ProductType);
            }

            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (categoryId > 0)
            {
                resultList = resultList.Where(r => r.ProductCategoryId == categoryId).ToList();
            }
            var contentsAdminViewModel = new ProductsAdminViewModel();
            contentsAdminViewModel.Products = resultList;
            contentsAdminViewModel.Categories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(storeId, ProductType);
            return View(contentsAdminViewModel);
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product content = ProductRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            if (!CheckRequest(content))
            {
                return RedirectToAction("NoAccessPage", "Home", new { id = content.StoreId });
            }
            return View(content);
        }

        //
        // GET: /Product/Create

        public ActionResult SaveOrEdit(int id = 0)
        {
            var content = new Product();
            if (id == 0)
            {
                content.Type = ProductType;
                content.CreatedDate = DateTime.Now;
            }
            else
            {
                content = ProductRepository.GetSingle(id);
                content.UpdatedDate = DateTime.Now;
                if (!CheckRequest(content))
                {
                    return RedirectToAction("NoAccessPage", "Home", new { id = content.StoreId });
                }
            }
            return View(content);
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Product content, int[] selectedFileId = null)
        {
            if (ModelState.IsValid)
            {
                content.Description = GetCleanHtml(content.Description);
                if (content.Id == 0)
                {
                    ProductRepository.Add(content);
                }
                else
                {
                    ProductRepository.Edit(content);
                }

                ProductRepository.Save();
                if (selectedFileId != null)
                {
                    int contentId = content.Id;
                    ProductFileRepository.SaveProductFiles(selectedFileId, contentId);
                }

                return RedirectToAction("Index");
            }

            return View(content);
        }



        //
        // GET: /Product/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id = 0)
        {
            Product content = ProductRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }
        public ActionResult StoreDetails(int id = 0)
        {
            Product product = ProductRepository.GetSingle(id);
            Store s = StoreRepository.GetSingle(product.StoreId);
            Category cat = CategoryRepository.GetSingle(product.ProductCategoryId);
            var productDetailLink = LinkHelper.GetProductLink(product, cat.Name);
            String detailPage = String.Format("http://{0}{1}", s.Domain, productDetailLink);
            return Redirect(detailPage);

        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product content = ProductRepository.GetSingle(id);
            ProductRepository.Delete(content);
            ProductRepository.Save();
            return RedirectToAction("Index");
        }


    }
}