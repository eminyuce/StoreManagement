using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class ActivitiesController : BaseController
    {



        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<Activity>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ActivityRepository.GetActivitiesByStoreId(storeId, search);
            }
            return View(resultList);
        }
        //
        // GET: /Activities/Details/5

        public ViewResult Details(int id)
        {
            Activity activity = ActivityRepository.GetSingle(id);
            return View(activity);
        }



        //
        // GET: /Brands/Edit/5

        public ActionResult SaveOrEdit(int id = 0,int selectedStoreId = 0)
        {
            Activity activity = new Activity();
            activity.StoreId = GetStoreId(selectedStoreId);
            if (id != 0)
            {
                activity = ActivityRepository.GetSingle(id);
                activity.UpdatedDate = DateTime.Now;
            }
            else
            {
                activity.CreatedDate = DateTime.Now;
                activity.State = true;
                activity.Ordering = 1;
                activity.UpdatedDate = DateTime.Now;
            }

            return View(activity);
        }

        //
        // POST: /Brands/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Activity activity)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (activity.Id == 0)
                    {
                        ActivityRepository.Add(activity);
                    }
                    else
                    {
                        ActivityRepository.Edit(activity);
                    }
                    ActivityRepository.Save();


                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = activity.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + activity);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(activity);
        }


        //
        // GET: /Activities/Delete/5

        public ActionResult Delete(int id)
        {
            Activity activity = ActivityRepository.GetSingle(id);
            return View(activity);
        }

        //
        // POST: /Activities/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = ActivityRepository.GetSingle(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            try
            {

                ActivityRepository.Delete(activity);
                ActivityRepository.Save();
                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = activity.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete:" + activity, ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(activity);
        }


    }
}