using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StoreManagement.Admin.Models;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{

    public class StoresController : BaseController
    {

        [AllowAnonymous]
        public PartialViewResult StoresFilter(String actionName = "", String controllerName = "")
        {
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_StoresFilter", StoreRepository.GetAll().ToList());
        }

        [AllowAnonymous]
        public PartialViewResult StoresDropDown(int storeId = 0)
        {
            ViewBag.StoreId = storeId;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.LoginStoreId = GetStoreId(storeId);
            return PartialView("_StoresDropDown", StoreRepository.GetAll().ToList());
        }
        [Authorize(Roles = "SuperAdmin")]
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
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SaveOrEdit(int id = 0)
        {
            var store = new Store();
            if (id != 0)
            {
                store = StoreRepository.GetSingle(id);
            }
            return View(store);
        }

        //
        // POST: /Stores/Edit/5

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SaveOrEdit(Store store)
        {
            if (ModelState.IsValid)
            {
                store.CreatedDate = DateTime.Now;
                if (store.Id == 0)
                {
                    StoreRepository.Add(store);
                }
                else
                {
                    StoreRepository.Edit(store);
                }

                StoreRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Stores/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }

        //
        // POST: /Stores/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreRepository.DeleteStore(id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
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

        // GET: /Stores/Details/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Users(int storeId, String search = "")
        {
            var store = this.StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;
            var storeUserIds = StoreUserRepository.FindBy(r => r.StoreId == storeId).Select(r => r.UserId).ToList();


            var storeUsers = (from u in DbContext.UserProfiles where storeUserIds.Contains(u.UserId) select u).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                storeUsers =
                    storeUsers.Where(r => r.UserName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            ViewBag.Roles = DbContext.Roles.ToList();
            return View(storeUsers.ToList());
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult DeleteStoreUser(int storeId = 0, String userName = "")
        {
            try
            {

                try
                {
                    UserProfile user = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
                    if (user != null)
                    {
                        var su = StoreUserRepository.GetStoreUserByUserId(user.UserId);
                        StoreUserRepository.Delete(su);
                        StoreUserRepository.Save();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception " + ex.Message, ex);
                }



                if (Roles.GetRolesForUser(userName).Any())
                {
                    Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
                }
                ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(userName); // deletes record from webpages_Membership table
                ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userName, true); // deletes record from UserProfile table




                return RedirectToAction("Users", new { storeId = storeId });

            }
            catch
            {
                return View(userName);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult StoreUserDetail(int storeId = 0, int userId = 0)
        {
            UserProfile storeUser = DbContext.UserProfiles.FirstOrDefault(r => r.UserId == userId);
            var store = StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;
            return View(storeUser);
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SaveOrEditStoreUser(int storeId = 0, int userId = 0)
        {
            var store = StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;

            ViewBag.Roles = DbContext.Roles.ToList();
            var loginModel = new LoginModel();
            if (userId > 0)
            {
                UserProfile storeUser = DbContext.UserProfiles.FirstOrDefault(r => r.UserId == userId);
                if (storeUser != null)
                {
                    loginModel.UserName = storeUser.UserName;
                    loginModel.FirstName = storeUser.FirstName;
                    loginModel.LastName = storeUser.LastName;
                    loginModel.PhoneNumber = storeUser.PhoneNumber;
                }
            }
            return View(loginModel);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult SaveOrEditStoreUser(int storeId, LoginModel userName, String roleName = "")
        {

            if (String.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("UserName", "SELECT A ROLE PLEASE");

            }
            var store = this.StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;

            ViewBag.Roles = DbContext.Roles.ToList();

            UserProfile user = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.UserName.ToLower());
            // Check if user already exists
            if (user == null)
            {

                WebSecurity.CreateUserAndAccount(userName.UserName, userName.Password);
                Roles.AddUserToRole(userName.UserName, roleName);

                var i = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.UserName.ToLower());

                i.FirstName = userName.FirstName;
                i.LastName = userName.LastName;
                i.PhoneNumber = userName.PhoneNumber;
                i.CreatedDate = DateTime.Now;
                i.LastLoginDate = DateTime.Now;
                DbContext.SaveChanges();

                if (!roleName.Equals("SuperAdmin", StringComparison.InvariantCultureIgnoreCase))
                {
                    StoreUser su = new StoreUser();
                    su.StoreId = storeId;
                    su.UserId = i.UserId;
                    su.State = true;
                    su.Ordering = 1;
                    su.CreatedDate = DateTime.Now;
                    su.UpdatedDate = DateTime.Now;

                    StoreUserRepository.Add(su);
                    StoreUserRepository.Save();
                }

            }
            else
            {

                user.UserName = userName.UserName;
                user.FirstName = userName.FirstName;
                user.LastName = userName.LastName;
                user.PhoneNumber = userName.PhoneNumber;
                DbContext.SaveChanges();
            }


            return RedirectToAction("Users", new { storeId = storeId });
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
            var settings = this.SettingRepository.GetStoreSettings(storeId);
            if (!String.IsNullOrEmpty(search))
            {
                settings = settings.Where(r => r.SettingKey.Contains(search.ToLower()) || r.SettingValue.Contains(search.ToLower())).ToList();
            }
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
