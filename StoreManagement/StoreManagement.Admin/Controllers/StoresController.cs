using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class StoresController : BaseController
    {
        //
        // GET: /Admin/
        private IStoreContext dbContext;
        private IStoreRepository storeRepository;
        private ISettingRepository settingRepository;
        private IStoreUserRepository storeUserRepository;

        public StoresController(IStoreContext dbContext, 
            IStoreRepository storeRepository, 
            ISettingRepository settingRepository,
            IStoreUserRepository storeUserRepository)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.storeRepository = storeRepository;
            this.settingRepository = settingRepository;
            this.storeUserRepository = storeUserRepository;
        }
        //
        // GET: /Stores/

        public ViewResult Index()
        {
            return View(storeRepository.GetAll().ToList());
        }
        //
        // GET: /Stores/Details/5
        public ViewResult Details(int id)
        {
            return View(storeRepository.GetSingle(id));
        }

        //
        // GET: /Stores/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Stores/Create

        [HttpPost]
        public ActionResult Create(Store store)
        {
            if (ModelState.IsValid)
            {
                storeRepository.Add(store);
                storeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Stores/Edit/5

        public ActionResult Edit(int id)
        {
            return View(storeRepository.GetSingle(id));
        }

        //
        // POST: /Stores/Edit/5

        [HttpPost]
        public ActionResult Edit(Store store)
        {
            if (ModelState.IsValid)
            {
                storeRepository.Edit(store);
                storeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Stores/Delete/5

        public ActionResult Delete(int id)
        {
            return View(storeRepository.GetSingle(id));
        }

        //
        // POST: /Stores/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            storeRepository.Delete(storeRepository.GetSingle(id));
            storeRepository.Save();
            return RedirectToAction("Index");
        }

        // GET: /Stores/Details/5
        public ActionResult Users(int id)
        {
            var store = this.storeRepository.GetSingle(id);
            ViewBag.Store = store;
            var storeUsers = storeUserRepository.FindBy(r => r.StoreId == id);
            return View();
        }
        // GET: /Stores/Details/5
        public ActionResult Settings(int id)
        {
            var store = this.storeRepository.GetSingle(id);
            ViewBag.Store = store;
            var settings = this.settingRepository.GetStoreSettings(id);
            return View(settings);
        }
        [HttpPost]
        public ActionResult SaveOrUpdateSetting(Setting setting)
        {
            if (ModelState.IsValid)
            {
                this.settingRepository.Add(setting);
                this.settingRepository.Save();
            }
            return RedirectToAction("Settings", new { id = setting.StoreId });
        }

    }
}
