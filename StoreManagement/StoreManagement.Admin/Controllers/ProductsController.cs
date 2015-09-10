using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var resultList = new List<Product>();
            storeId = GetStoreId(storeId);
            if (storeId != 0 && (categoryId != 0 || !String.IsNullOrEmpty(search)))
            {
                resultList = ProductRepository.GetProductByTypeAndCategoryId(storeId, StoreConstants.ProductType, categoryId, search);
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
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0, int selectedCategoryId = 0)
        {


            var content = new Product();
            content.ProductCategoryId = selectedCategoryId;
            content.StoreId = GetStoreId(selectedStoreId);

            var labels = new List<LabelLine>();
            var fileManagers = new List<FileManager>();
            int mainImageFileManagerId = 0;
            if (id == 0)
            {
                content.Type = StoreConstants.ProductType;
                content.CreatedDate = DateTime.Now;
                content.State = true;
                content.UpdatedDate = DateTime.Now;
            }
            else
            {

                content = ProductRepository.GetSingleIncluding(id, r => r.ProductFiles.Select(r1 => r1.FileManager));
                if (!CheckRequest(content)) //security for right store and its item.
                {
                    return HttpNotFound("Not Found");
                }
                content.UpdatedDate = DateTime.Now;
                labels = LabelLineRepository.GetLabelLinesByItem(id, StoreConstants.ProductType);

                fileManagers = content.ProductFiles.Select(r => r.FileManager).ToList();
                var mainImage = content.ProductFiles.FirstOrDefault(r => r.IsMainImage);
                if (mainImage != null)
                {
                    mainImageFileManagerId = mainImage.FileManagerId;
                }
            }
            ViewBag.LabelLines = labels;
            ViewBag.FileManagers = fileManagers;
            ViewBag.MainImageId = mainImageFileManagerId;

            return View(content);
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Product product, int[] selectedFileId = null, int[] selectedLabelId = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!CheckRequest(product))
                    {
                        return new HttpNotFoundResult("Not Found");
                    }


                    if (product.ProductCategoryId == 0)
                    {
                        int mainImageFileManagerId = 0;
                        List<FileManager> fileManagers = new List<FileManager>();
                        var labelList = new List<LabelLine>();
                        if (product.Id > 0)
                        {
                            var content = ProductRepository.GetSingleIncluding(product.Id, r => r.ProductFiles.Select(r1 => r1.FileManager));
                            fileManagers = content.ProductFiles.Select(r => r.FileManager).ToList();
                            labelList = LabelLineRepository.GetLabelLinesByItem(product.Id, StoreConstants.ProductType);
                            var mainImage = content.ProductFiles.FirstOrDefault(r => r.IsMainImage);
                            if (mainImage != null)
                            {
                                mainImageFileManagerId = mainImage.FileManagerId;
                            }
                        }
                        ViewBag.FileManagers = fileManagers;
                        ViewBag.LabelLines = labelList;
                        ViewBag.MainImageId = mainImageFileManagerId;
                        ModelState.AddModelError("ProductCategoryId", "You should select category from category tree.");
                        return View(product);
                    }


                    product.Description = GetCleanHtml(product.Description);
                    if (product.Id == 0)
                    {
                        ProductRepository.Add(product);
                    }
                    else
                    {
                        ProductRepository.Edit(product);
                    }

                    ProductRepository.Save();
                    int contentId = product.Id;
                    if (selectedFileId != null)
                    {
                        ProductFileRepository.SaveProductFiles(selectedFileId, contentId);
                    }


                    LabelLineRepository.SaveLabelLines(selectedLabelId, contentId, StoreConstants.ProductType);


                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = product.StoreId, categoryId = product.ProductCategoryId });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { categoryId = product.ProductCategoryId });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to save changes:" + product, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(product);
        }



        //
        // GET: /Product/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product content = ProductRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            if (!CheckRequest(content))
            {
                return new HttpNotFoundResult("Not Found");
            }
            
            return View(content);
        }
        public ActionResult StoreDetails(int id = 0)
        {
            try
            {
                Product product = ProductRepository.GetSingle(id);
                Store s = StoreRepository.GetSingle(product.StoreId);
                ProductCategory cat = ProductCategoryRepository.GetSingle(product.ProductCategoryId);
                var productDetailLink = LinkHelper.GetProductLink(product, cat.Name);
                String detailPage = String.Format("http://{0}{1}", s.Domain, productDetailLink);

                return Redirect(detailPage);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new EmptyResult();
            }
            

        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {

            Product product = ProductRepository.GetSingle(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            try
            {
                if (!CheckRequest(product))
                {
                    return new HttpNotFoundResult("Not Found");
                }

                ProductRepository.Delete(product);
                ProductRepository.Save();
                LabelLineRepository.DeleteLabelLinesByItem(product.Id, StoreConstants.ProductType);
                ProductFileRepository.DeleteProductFileByProductId(product.Id);

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = product.StoreId, categoryId = product.ProductCategoryId });
                }
                else
                {
                    return RedirectToAction("Index", new { categoryId = product.ProductCategoryId });
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to delete product:" + product, ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(product);



        }


    }
}