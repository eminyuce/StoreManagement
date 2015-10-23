using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class StorePageDesignsController : BaseController
    {


        [AllowAnonymous]
        public PartialViewResult StorePageDesignsFilter(String actionName = "", String controllerName = "")
        {
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_StorePageDesignsFilter", StorePageDesignRepository.GetActiveStoreDesings());
        }

        public ActionResult Index(String search = "")
        {
            var resultList = new List<StorePageDesign>();
            resultList = StorePageDesignRepository.GetStorePageDesignsByStoreId(search);
            return View(resultList);
        }

        //
        // GET: /StorePageDesigns/Details/5

        public ViewResult Details(int id)
        {
            StorePageDesign storePageDesign = StorePageDesignRepository.GetSingle(id);
            return View(storePageDesign);
        }

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var storePageDesign = new StorePageDesign();

            if (id != 0)
            {
                storePageDesign = StorePageDesignRepository.GetSingle(id);
                storePageDesign.UpdatedDate = DateTime.Now;
            }
            else
            {
                storePageDesign.UpdatedDate = DateTime.Now;
                storePageDesign.CreatedDate = DateTime.Now;
                storePageDesign.State = true;

            }
            return View(storePageDesign);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(StorePageDesign storePageDesign)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (storePageDesign.Id == 0)
                    {
                        StorePageDesignRepository.Add(storePageDesign);
                    }
                    else
                    {
                        StorePageDesignRepository.Edit(storePageDesign);
                    }
                    StorePageDesignRepository.Save();
                    return RedirectToAction("Index");
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, storePageDesign);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(storePageDesign);
        }

        //
        // GET: /StorePageDesigns/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            StorePageDesign storePageDesign = StorePageDesignRepository.GetSingle(id);
            return View(storePageDesign);
        }

        //
        // POST: /StorePageDesigns/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StorePageDesign storePageDesign = StorePageDesignRepository.GetSingle(id);
            try
            {
                StorePageDesignRepository.Delete(storePageDesign);
                StorePageDesignRepository.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, storePageDesign);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(storePageDesign);
        }

	}
}