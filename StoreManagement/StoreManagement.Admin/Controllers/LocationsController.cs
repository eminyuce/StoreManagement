using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class LocationsController : BaseController
    {

        //
        // GET: /Locations/


        public ActionResult Index(int storeId = 0, String search = "")
        {
            List<Location> resultList = new List<Location>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = LocationRepository.GetLocationsByStoreId(storeId, search);
            }
            return View(resultList);
        }

        //
        // GET: /Locations/Details/5

        public ViewResult Details(int id)
        {
            Location location = LocationRepository.GetSingle(id);
            return View(location);
        }

        //
        // GET: /Locations/Edit/5

        public ActionResult SaveOrEdit(int id = 0)
        {
            Location location = new Location();

            if (id != 0)
            {
                location = LocationRepository.GetSingle(id);
            }

            return View(location);
        }

        //
        // POST: /Locations/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Location location)
        {
            if (ModelState.IsValid)
            {
                location.Latitude = 1;
                location.Longitude = 1;
                if (location.Id > 0)
                {
                    location.UpdatedDate = DateTime.Now;
                    LocationRepository.Edit(location);
                }
                else
                {
                    location.State = true;
                    location.UpdatedDate = DateTime.Now;
                    location.CreatedDate = DateTime.Now;
                    LocationRepository.Add(location);
                }

                LocationRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = location.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(location);
        }

        //
        // GET: /Locations/Delete/5

        public ActionResult Delete(int id)
        {
            Location location = LocationRepository.GetSingle(id);
            return View(location);
        }

        //
        // POST: /Locations/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = LocationRepository.GetSingle(id);
            LocationRepository.Delete(location);
            LocationRepository.Save();

            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = location.StoreId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}