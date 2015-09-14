using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Quartz.Util;
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
            //StoreRepository.CopyStore(copyStoreId, name, domain, layout);
            int newStoreId = 0;

            try
            {
                var store = StoreRepository.GetStore(copyStoreId);
                var storeCopy = GeneralHelper.DataContractSerialization(store);
                storeCopy.Id = 0;
                storeCopy.Name = name;
                storeCopy.Domain = domain;
                storeCopy.GoogleDriveFolder = domain;
                StoreRepository.Add(storeCopy);
                StoreRepository.Save();

                newStoreId = storeCopy.Id;


                try
                {
                    var settingsStore = SettingRepository.GetStoreSettings(copyStoreId);
                    foreach (var settingStore in settingsStore)
                    {
                        var s = GeneralHelper.DataContractSerialization(settingStore);
                        s.Id = 0;
                        s.StoreId = newStoreId;
                        SettingRepository.Add(s);
                    }
                    SettingRepository.Save();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CopyStore", newStoreId);
                }


                try
                {
                    var pageDesings = PageDesignRepository.GetPageDesignByStoreId(copyStoreId, "");
                    foreach (var pageDesing in pageDesings)
                    {
                        var s = GeneralHelper.DataContractSerialization(pageDesing);
                        s.Id = 0;
                        s.StoreId = newStoreId;
                        PageDesignRepository.Add(s);
                    }
                    PageDesignRepository.Save();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "CopyStore", newStoreId);
                }


                var res = Task.Factory.StartNew(() => CopyStoreData(copyStoreId, newStoreId));


                return RedirectToAction("Index", new { search = name.ToLower() });

            }
            catch (Exception exc)
            {

                Logger.Error(exc, "CopyStore", copyStoreId, name, domain);
            }


            return RedirectToAction("CopyStore", new { id = copyStoreId });




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
                this.SettingRepository.SaveSetting();
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

                    setting.CreatedDate = DateTime.Now;
                    setting.UpdatedDate = DateTime.Now;
                    SettingRepository.Add(setting);
                }
                else
                {

                    setting.UpdatedDate = DateTime.Now;
                    SettingRepository.Edit(setting);
                }


                SettingRepository.SaveSetting();
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
            SettingRepository.SaveSetting();
            return RedirectToAction("Settings", new { storeId = setting.StoreId });
        }
        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("DeleteStoreSettings")]
        public ActionResult DeleteStoreSettingsConfirmed(int id)
        {
            Setting setting = SettingRepository.GetSingle(id);
            SettingRepository.Delete(setting);
            SettingRepository.SaveSetting();
            return RedirectToAction("Index");
        }

        #region CopyStoreMethod




        private void CopyStoreData(int copyStoreId, int newStoreId)
        {
            StoreDbContext.Configuration.ProxyCreationEnabled = false;


            try
            {
                var items = NavigationRepository.GetNavigationsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    NavigationRepository.Add(s);
                }
                NavigationRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
            try
            {
                var items = LocationRepository.GetLocationsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    LocationRepository.Add(s);
                }
                LocationRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
            try
            {
                var items = EmailListRepository.GetStoreEmailList(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    EmailListRepository.Add(s);
                }
                EmailListRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
            try
            {
                var items = BrandRepository.GetBrandsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    BrandRepository.Add(s);
                }
                BrandRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
            try
            {
                var items = ContactRepository.GetContactsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    ContactRepository.Add(s);
                }
                ContactRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
            int productCategoryId = 0;
            try
            {
                var items = ProductCategoryRepository.GetProductCategoriesByStoreId(copyStoreId);
                foreach (var productCategory in items)
                {
                    var s = GeneralHelper.DataContractSerialization(productCategory);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    ProductCategoryRepository.Add(s);
                    ProductCategoryRepository.Save();

                    productCategoryId = s.Id;


                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductCategoryRepository:CopyStore");
            }

            int blogCategoryId = 0;
            int newsCategoryId = 0;
            try
            {

                var items = CategoryRepository.GetCategoriesByStoreId(copyStoreId, StoreConstants.BlogsType);
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    CategoryRepository.Add(s);
                    CategoryRepository.Save();

                    blogCategoryId = s.Id;

                }

                items = CategoryRepository.GetCategoriesByStoreId(copyStoreId, StoreConstants.NewsType);
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    CategoryRepository.Add(s);
                    CategoryRepository.Save();

                    newsCategoryId = s.Id;

                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }


            try
            {

                var items = ProductRepository.GetProductsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.ProductCategoryId = productCategoryId;
                    s.StoreId = newStoreId;
                    ProductRepository.Add(s);
                    ProductRepository.Save();



                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }

            try
            {
                var items = ContentRepository.GetContentsByStoreId(copyStoreId, "", StoreConstants.BlogsType);
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    s.CategoryId = blogCategoryId;
                    ContentRepository.Add(s);
                    ContentRepository.Save();
                }

                items = ContentRepository.GetContentsByStoreId(copyStoreId, "", StoreConstants.NewsType);
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    s.CategoryId = newsCategoryId;
                    ContentRepository.Add(s);
                    ContentRepository.Save();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }

            try
            {

                var items = LabelRepository.GetLabelsByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    LabelRepository.Add(s);
                    LabelRepository.Save();

                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }

            try
            {

                var items = ActivityRepository.GetActivitiesByStoreId(copyStoreId, "");
                foreach (var item in items)
                {
                    var s = GeneralHelper.DataContractSerialization(item);
                    s.Id = 0;
                    s.StoreId = newStoreId;
                    ActivityRepository.Add(s);
                    ActivityRepository.Save();

                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CopyStore", newStoreId);
            }
        }



        #endregion
    }
}
