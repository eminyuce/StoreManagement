using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class SettingsController : BaseController
    {

        //
        // GET: /Setting/
        public SettingsController(IStoreContext dbContext, ISettingRepository settingRepository)
            : base(dbContext, settingRepository)
        {

        }

        //
        // GET: /Settings/

        public ViewResult Index()
        {
            return View(settingRepository.GetAll());
        }

        //
        // GET: /Settings/Details/5

        public ViewResult Details(int id)
        {
            Setting setting = settingRepository.GetSingle(id);
            return View(setting);
        }

        //
        // GET: /Settings/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Settings/Create

        [HttpPost]
        public ActionResult Create(Setting setting)
        {
            if (ModelState.IsValid)
            {
                setting.SettingKey = setting.SettingKey.ToLower();
                settingRepository.Add(setting);
                settingRepository.Save();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        //
        // GET: /Settings/Edit/5

        public ActionResult Edit(int id)
        {
            Setting setting = settingRepository.GetSingle(id);
            return View(setting);
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult Edit(Setting setting)
        {
            if (ModelState.IsValid)
            {
                setting.SettingKey = setting.SettingKey.ToLower();
                settingRepository.Edit(setting);
                settingRepository.Save();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        //
        // GET: /Settings/Delete/5

        public ActionResult Delete(int id)
        {
            Setting setting = settingRepository.GetSingle(id);
            return View(setting);
        }

        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting setting = settingRepository.GetSingle(id);
            settingRepository.Delete(setting);
            settingRepository.Save();
            return RedirectToAction("Index");
        }
        public ActionResult StoreSettings(int storeId)
        {
            return View(settingRepository.GetStoreSettings(storeId));
        }

	}
}