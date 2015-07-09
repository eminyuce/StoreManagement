using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StoreManagement.Admin.Models;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class StoresController : UsersController
    {

        [AllowAnonymous]
        public PartialViewResult StoresFilter(String actionName = "", String controllerName = "")
        {
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_StoresFilter", StoreRepository.GetAllStores());
        }

        [AllowAnonymous]
        public PartialViewResult StoresDropDown(int storeId = 0)
        {
            ViewBag.StoreId = storeId;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.LoginStoreId = GetStoreId(storeId);
            return PartialView("_StoresDropDown", StoreRepository.GetAllStores());
        }
        public ViewResult Index(String search = "", int categoryId = 0)
        {

            var resultList = new List<Store>();
            resultList = StoreRepository.GetAll().ToList();

            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (categoryId > 0)
            {
                resultList = resultList.Where(r => r.CategoryId == categoryId).ToList();
            }

            resultList = resultList.OrderBy(r => r.Domain).ToList();

            return View(resultList);
        }
        //
        // GET: /Stores/Details/5
        [Authorize(Roles = "SuperAdmin")]
        public ViewResult Details(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }




        //
        // GET: /Stores/Edit/5
        public ActionResult SaveOrEdit(int id = 0)
        {
            var store = new Store();
            if (id != 0)
            {
                store = StoreRepository.GetSingle(id);
            }

            ViewBag.StoreCategories = CategoryRepository.GetCategoriesByType(StoreConstants.StoreType);
            return View(store);
        }

        //
        // POST: /Stores/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Store store, HttpPostedFileBase certUpload = null)
        {

            ViewBag.StoreCategories = CategoryRepository.GetCategoriesByType(StoreConstants.StoreType);
            if (ModelState.IsValid)
            {
                store.CreatedDate = DateTime.Now;

                if (certUpload != null)
                {
                    store.GoogleDriveCertificateP12FileName = Path.GetFileName(certUpload.FileName);
                    store.GoogleDriveCertificateP12RawData = GeneralHelper.ToByteArray(certUpload);
                }
                else
                {
                    if (store.Id != 0)
                    {
                      //  var oldStore = StoreRepository.GetSingle(store.Id);
                      //  store.GoogleDriveCertificateP12RawData = oldStore.GoogleDriveCertificateP12RawData;
                    }
                }


                if (store.Id == 0)
                {
                    store.UpdatedDate = DateTime.Now;
                    store.CreatedDate = DateTime.Now;
                    StoreRepository.Add(store);
                }
                else
                {
                
                    StoreRepository.Edit(store);
                    store.UpdatedDate = DateTime.Now;
                }

                StoreRepository.SaveStore();
                return RedirectToAction("Index");
            }
            else
            {
                return View(store);
            }
        }
        [HttpGet]
        public ActionResult CopyStore(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }
        [HttpPost]
        public ActionResult CopyStore(int copyStoreId, String name, String domain)
        {
            String layout = "";
            StoreRepository.CopyStore(copyStoreId, name, domain, layout);
 
            return RedirectToAction("Index", new { search = name.ToLower() });
        }

        //
        // GET: /Stores/Delete/5
        public ActionResult Delete(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }

        //
        // POST: /Stores/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreRepository.DeleteStore(id);
            return RedirectToAction("Index");
        }
        public ActionResult SaveStoreUsers(int id, LoginModel userName, String roleName)
        {


            var regexUtil = new RegexUtilities();
            if (!regexUtil.IsValidEmail(userName.UserName))
            {
                ModelState.AddModelError("UserName", "Invalid Email Address");
                return View(userName);
            }


            int storeId = id;
            //if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(userName.UserName, userName.Password);
                    Roles.AddUserToRole(userName.UserName, roleName);
                    int userId = 0;
                    // using (UsersContext db = new UsersContext())
                    {
                        UserProfile user = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.UserName.ToLower());
                        userId = user.UserId;
                        user.FirstName = userName.FirstName;
                        user.LastName = userName.LastName;
                        user.PhoneNumber = userName.PhoneNumber;
                        user.CreatedDate = DateTime.Now;
                        DbContext.SaveChanges();

                    }

                    if (!roleName.Equals("SuperAdmin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var su = new StoreUser();
                        su.StoreId = storeId;
                        su.UserId = userId;
                        su.CreatedDate = DateTime.Now;
                        su.UpdatedDate = DateTime.Now;

                        StoreUserRepository.Add(su);
                        StoreUserRepository.Save();
                    }


                    return RedirectToAction("Users", new { id = storeId });
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", "Exception:" + e.Message);
                }
            }
            return RedirectToAction("Users", new { id = storeId });
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SaveOrUpdateSetting(Setting setting)
        {
            if (ModelState.IsValid)
            {
                this.SettingRepository.Add(setting);
                this.SettingRepository.Save();
            }
            return RedirectToAction("Settings", new { id = setting.StoreId });
        }


        // GET: /Stores/Details/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Settings(int storeId, String search = "")
        {
            var store = this.StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;
            var settings = this.SettingRepository.GetStoreSettingsByType(storeId, StoreConstants.SuperSettings, search);

            return View(settings);
        }


        //
        // GET: /Settings/Edit/5

        public ActionResult SaveOrEditStoreSettings(int storeId = 0, int settingId = 0)
        {
            Setting setting = new Setting();
            setting.State = true;
            if (settingId != 0)
            {
                setting = SettingRepository.GetSingle(settingId);
            }
            else
            {
                setting.Type = "SuperSettings";
                setting.StoreId = storeId;

            }

            return View(setting);
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult SaveOrEditStoreSettings(Setting setting)
        {
            if (ModelState.IsValid)
            {
                setting.Type = "SuperSettings";
                if (setting.Id == 0)
                {
                    setting.SettingKey = setting.SettingKey.ToLower();
                    setting.CreatedDate = DateTime.Now;
                    setting.UpdatedDate = DateTime.Now;
                    SettingRepository.Add(setting);
                }
                else
                {
                    setting.SettingKey = setting.SettingKey.ToLower();
                    setting.UpdatedDate = DateTime.Now;
                    SettingRepository.Edit(setting);
                }


                SettingRepository.Save();
                return RedirectToAction("Settings", new { storeId = setting.StoreId });
            }
            return View(setting);
        }

        //
        // GET: /Settings/Delete/5

        public ActionResult DeleteStoreSettings(int storeId = 0, int settingId = 0)
        {
            Setting setting = SettingRepository.GetSingle(settingId);
            SettingRepository.Delete(setting);
            SettingRepository.Save();
            return RedirectToAction("Settings", new { storeId = setting.StoreId });
        }
        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("DeleteStoreSettings")]
        public ActionResult DeleteStoreSettingsConfirmed(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
            SettingRepository.Delete(setting);
            SettingRepository.Save();
            return RedirectToAction("Index");
        }


    }
}
