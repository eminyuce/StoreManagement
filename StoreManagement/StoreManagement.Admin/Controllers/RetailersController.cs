using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class RetailersController : BaseController
    {


        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<Retailer>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = RetailerRepository.GetRetailersByStoreId(storeId, search);
            }

            return View(resultList);
        }

        //
        // GET: /Retailers/Details/5

        public ViewResult Details(int id)
        {
            Retailer retailer = RetailerRepository.GetSingle(id);
            return View(retailer);
        }

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var retailer = new Retailer();
            retailer.StoreId = GetStoreId(selectedStoreId);
            if (id != 0)
            {
                retailer = RetailerRepository.GetSingle(id);
                retailer.UpdatedDate = DateTime.Now;
            }
            else
            {
                retailer.UpdatedDate = DateTime.Now;
                retailer.CreatedDate = DateTime.Now;
                retailer.State = true;

            }
            return View(retailer);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(Retailer retailer)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (retailer.Id == 0)
                    {
                        RetailerRepository.Add(retailer);
                    }
                    else
                    {
                        RetailerRepository.Edit(retailer);
                    }
                    RetailerRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = retailer.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, retailer);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(retailer);
        }

        //
        // GET: /Retailers/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            Retailer retailer = RetailerRepository.GetSingle(id);
            return View(retailer);
        }

        //
        // POST: /Retailers/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Retailer retailer = RetailerRepository.GetSingle(id);
            try
            {
                RetailerRepository.Delete(retailer);
                RetailerRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = retailer.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, retailer);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(retailer);
        }



	}
}