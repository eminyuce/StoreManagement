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

        public ViewResult Index()
        {
            return View(LocationRepository.GetAll());
        }

        //
        // GET: /Locations/Details/5

        public ViewResult Details(int id)
        {
            Location location = LocationRepository.GetSingle(id);
            return View(location);
        }

        //
        // GET: /Locations/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Locations/Create

        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                LocationRepository.Add(location);
                LocationRepository.Save();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        //
        // GET: /Locations/Edit/5

        public ActionResult Edit(int id)
        {
            Location location = LocationRepository.GetSingle(id);
            return View(location);
        }

        //
        // POST: /Locations/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                LocationRepository.Edit(location);
                LocationRepository.Save();
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

	}
}