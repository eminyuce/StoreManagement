using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

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
            try
            {
                if (ModelState.IsValid)
                {

                    String address = location.Address.ToStr() + " "
                  + location.City.ToStr() + " "
                  + location.State.ToStr() + " "
                  + location.Postal.ToStr() + " "
                  + location.Country.ToStr() + " ";
                    address = address.Trim();
                    location.Name = address;
                    if (!String.IsNullOrEmpty(address))
                    {
                        var result = LatitudeAndLongitudeParser.GetLatitudeAndLongitude(address);
                        if (result.Count > 0)
                        {
                            location.Longitude = result[1];
                            location.Latitude = result[0];
                        }
                    }


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
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save changes:" + location);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
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

            try
            {
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
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to delete:" + location, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(location);

        }

    }
}