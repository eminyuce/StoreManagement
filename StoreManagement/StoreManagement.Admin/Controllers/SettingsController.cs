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
    [Authorize]
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

        public ViewResult Index(int storeId= 0, String type="")
        {
            storeId = GetStoreId(storeId);
            List<Setting> items = null;
            if (!String.IsNullOrEmpty(type))
            {
                items = SettingRepository.GetStoreSettingsByType(1, type);
            }
            else
            {
                items = SettingRepository.GetAll().ToList();
            }
            var types = from p in SettingRepository.GetAll()
                        where !String.IsNullOrEmpty(p.Type) 
                        group p by p.Type into g
                        select new { Type = g.Key };

            ViewBag.Types = types.Select(r => r.Type).ToList();

            return View(items);
        }

        //
        // GET: /Settings/Details/5

        public ViewResult Details(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
            return View(setting);
        }

        //
        // GET: /Settings/Create

        public ActionResult Create()
        {
            Setting setting=new Setting();
            setting.StoreId = 1;
            setting.State = true;
            return View(setting);
        }

        //
        // POST: /Settings/Create

        [HttpPost]
        public ActionResult Create(Setting setting)
        {
            if (ModelState.IsValid)
            {
                setting.SettingKey = setting.SettingKey.ToLower();
                SettingRepository.Add(setting);
                SettingRepository.Save();
                return RedirectToAction("Index");
            }

            return View(setting);
        }

        //
        // GET: /Settings/Edit/5

        public ActionResult Edit(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
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
                SettingRepository.Edit(setting);
                SettingRepository.Save();
                return RedirectToAction("Index");
            }
            return View(setting);
        }

        //
        // GET: /Settings/Delete/5

        public ActionResult Delete(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
            return View(setting);
        }

        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
            SettingRepository.Delete(setting);
            SettingRepository.Save();
            return RedirectToAction("Index");
        }
        public ActionResult StoreSettings(int storeId)
        {
            return View(SettingRepository.GetStoreSettings(storeId));
        }


        public ActionResult TestSetting(int id=1)
        {
            ViewBag.StoreId = id;
            var settings = SettingRepository.GetStoreSettings(id).Where(r => r.Type.ToLower().Contains("Style".ToLower())).ToList();
            return View(settings);
        }
	}
}