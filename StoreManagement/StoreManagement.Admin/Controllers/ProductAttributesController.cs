using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class ProductAttributesController : BaseController
    {
        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<ProductAttribute>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ProductAttributeRepository.GetProductAttributesByStoreId(storeId, search);
            }

            return View(resultList);
        }

        //
        // GET: /ProductAttributes/Details/5

        public ViewResult Details(int id)
        {
            ProductAttribute productAttribute = ProductAttributeRepository.GetSingle(id);
            return View(productAttribute);
        }

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var productAttribute = new ProductAttribute();
            productAttribute.StoreId = GetStoreId(selectedStoreId);
            if (id != 0)
            {
                productAttribute = ProductAttributeRepository.GetSingle(id);
                productAttribute.UpdatedDate = DateTime.Now;
            }
            else
            {
                productAttribute.UpdatedDate = DateTime.Now;
                productAttribute.CreatedDate = DateTime.Now;
                productAttribute.State = true;

            }
            return View(productAttribute);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(ProductAttribute productAttribute)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (productAttribute.Id == 0)
                    {
                        ProductAttributeRepository.Add(productAttribute);
                    }
                    else
                    {
                        ProductAttributeRepository.Edit(productAttribute);
                    }
                    ProductAttributeRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = productAttribute.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, productAttribute);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(productAttribute);
        }

        //
        // GET: /ProductAttributes/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            ProductAttribute productAttribute = ProductAttributeRepository.GetSingle(id);
            return View(productAttribute);
        }

        //
        // POST: /ProductAttributes/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductAttribute productAttribute = ProductAttributeRepository.GetSingle(id);
            try
            {
                ProductAttributeRepository.Delete(productAttribute);
                ProductAttributeRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = productAttribute.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, productAttribute);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(productAttribute);
        }

	}
}