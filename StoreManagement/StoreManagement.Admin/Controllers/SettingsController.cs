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
    [Authorize(Roles = "StoreAdmin,SuperAdmin")]
    public class SettingsController : BaseController
    {
        private const String TYPE = "StoreSettings";


        //
        // GET: /Settings/

        public ViewResult Index(int storeId = 0)
        {

            storeId = GetStoreId(storeId);
            List<Setting> items = null;
            items = SettingRepository.GetStoreSettingsByType(storeId, TYPE);
            return View(items);
        }

        //
        // GET: /Settings/Edit/5

        public ActionResult SaveOrEdit(int id = 0)
        {
            Setting setting = new Setting();
            setting.State = true;
            if (id != 0)
            {
                setting = SettingRepository.GetSingle(id);
            }
            setting.Type = TYPE;
            setting.StoreId = GetStoreId(0);
            return View(setting);
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Setting setting)
        {
            if (ModelState.IsValid)
            {
                setting.Type = TYPE;
                setting.StoreId = GetStoreId(0);
                if (setting.Id == 0)
                {
                    setting.SettingKey = setting.SettingKey.ToLower();
                    SettingRepository.Add(setting);
                }
                else
                {
                    setting.SettingKey = setting.SettingKey.ToLower();
                    SettingRepository.Edit(setting);
                }


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

        public ActionResult TestSetting(int id = 1)
        {
            ViewBag.StoreId = id;
            var settings = SettingRepository.GetStoreSettings(id).Where(r => r.Type.ToLower().Contains("Style".ToLower())).ToList();
            return View(settings);
        }
    }
}