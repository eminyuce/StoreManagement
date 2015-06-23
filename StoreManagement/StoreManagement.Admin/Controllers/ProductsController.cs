using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using StoreManagement.Data.Constants;
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
     


        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            List<Product> resultList = new List<Product>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ProductRepository.GetProductByType(storeId, StoreConstants.ProductType);
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
            contentsAdminViewModel.Categories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(storeId, StoreConstants.ProductType);
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
            var labels = new List<LabelLine>();
            if (id == 0)
            {
                content.Type = StoreConstants.ProductType;
                content.CreatedDate = DateTime.Now;
                content.State = true;
            }
            else
            {
                content = ProductRepository.GetSingle(id);
                content.UpdatedDate = DateTime.Now;
                if (!CheckRequest(content))
                {
                    return RedirectToAction("NoAccessPage", "Home", new { id = content.StoreId });
                }
                labels = LabelLineRepository.GetLabelLinesByItem(id, StoreConstants.ProductType);
            }
            ViewBag.SelectedLabels = labels.Select(r => r.LabelId).ToArray();
            return View(content);
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Product content, int[] selectedFileId = null, int[] selectedLabelId = null)
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
                int contentId = content.Id;
                if (selectedFileId != null)
                {
                    ProductFileRepository.SaveProductFiles(selectedFileId, contentId);
                }
                LabelLineRepository.SaveLabelLines(selectedLabelId, contentId, StoreConstants.ProductType);
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