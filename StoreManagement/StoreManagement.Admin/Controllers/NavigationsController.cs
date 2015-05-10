using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class NavigationsController : BaseController
    {
        private INavigationRepository navigationRepository;

        public NavigationsController(IStoreContext dbContext,
       ISettingRepository settingRepository,
       INavigationRepository navigationRepository)
            : base(dbContext, settingRepository)
        {
            this.navigationRepository = navigationRepository;
        }

        //
        // GET: /Navigations/

        public ViewResult Index()
        {
            return View(navigationRepository.GetStoreNavigation(1));
        }

        //
        // GET: /Navigations/Details/5

        public ViewResult Details(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // GET: /Navigations/Create

        public ActionResult Create()
        {
            var item = new Navigation();
            item.StoreId = 1;
            return View();
        }

        //
        // POST: /Navigations/Create

        [HttpPost]
        public ActionResult Create(Navigation navigation)
        {
            if (ModelState.IsValid)
            {
                navigationRepository.Add(navigation);
                navigationRepository.Save();
                return RedirectToAction("Index");
            }

            return View(navigation);
        }

        //
        // GET: /Navigations/Edit/5

        public ActionResult Edit(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // POST: /Navigations/Edit/5

        [HttpPost]
        public ActionResult Edit(Navigation navigation)
        {
            if (ModelState.IsValid)
            {
                navigationRepository.Edit(navigation);
                navigationRepository.Save();
                return RedirectToAction("Index");
            }
            return View(navigation);
        }

        //
        // GET: /Navigations/Delete/5

        public ActionResult Delete(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // POST: /Navigations/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Navigation navigation = navigationRepository.GetSingle(id);
            navigationRepository.Delete(navigation);
            navigationRepository.Save();
            return RedirectToAction("Index");
        }

    }
}