using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class BrandsController : BaseController
    {

        //
        // GET: /Brands/

        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<Brand>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = BrandRepository.GetBrandsByStoreId(storeId, search);
            }

            return View(resultList);
        }
        //
        // GET: /Brands/Details/5

        public ViewResult Details(int id)
        {
            Brand brand = BrandRepository.GetSingle(id);
            return View(brand);
        }



        //
        // GET: /Brands/Edit/5

        public ActionResult SaveOrEdit(int id = 0)
        {
            Brand brand = new Brand();
            if (id != 0)
            {
                brand = BrandRepository.GetSingle(id);
                brand.UpdatedDate = DateTime.Now;
            }
            else
            {
                brand.CreatedDate = DateTime.Now;
                brand.State = true;
                brand.Ordering = 1;
                brand.UpdatedDate = DateTime.Now;
            }

            return View(brand);
        }

        //
        // POST: /Brands/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Brand brand)
        {
            if (ModelState.IsValid)
            {

                if (brand.Id == 0)
                {
                    BrandRepository.Add(brand);
                }
                else
                {
                    BrandRepository.Edit(brand);
                }
                BrandRepository.Save();


                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = brand.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(brand);
        }

        //
        // GET: /Brands/Delete/5

        public ActionResult Delete(int id)
        {
            Brand brand = BrandRepository.GetSingle(id);
            return View(brand);
        }

        //
        // POST: /Brands/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = BrandRepository.GetSingle(id);
            BrandRepository.Delete(brand);
            BrandRepository.Save();
            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = brand.StoreId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}