using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StoreManagement.Admin.Models;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class StoresController : BaseController
    {
       
        public StoresController(IStoreContext dbContext,
            ISettingRepository settingRepository)
            : base(dbContext, settingRepository)
        {
            
        }
        public PartialViewResult StoresFilter(String actionName = "", String controllerName = "")
        {
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            return PartialView("_StoresFilter", StoreRepository.GetAll().ToList());
        }


        public PartialViewResult StoresDropDown(int storeId = 0)
        {
            ViewBag.StoreId = storeId;
            return PartialView("_StoresDropDown", StoreRepository.GetAll().ToList());
        }
        public ViewResult Index()
        {
            return View(StoreRepository.GetAll().ToList());
        }
        //
        // GET: /Stores/Details/5
        public ViewResult Details(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }

 
 

        //
        // GET: /Stores/Edit/5

        public ActionResult SaveOrEdit(int id=0)
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

        public ActionResult Delete(int id)
        {
            return View(StoreRepository.GetSingle(id));
        }

        //
        // POST: /Stores/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreRepository.Delete(StoreRepository.GetSingle(id));
            StoreRepository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult SaveStoreUsers(int id, LoginModel userName, String roleName)
        {
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

                    var su = new StoreUser();
                    su.StoreId = storeId;
                    su.UserId = userId;
                    StoreUserRepository.Add(su);
                    StoreUserRepository.Save();

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
        public ActionResult Users(int id)
        {
            var store = this.StoreRepository.GetSingle(id);
            ViewBag.Store = store;
            var storeUserIds = StoreUserRepository.FindBy(r => r.StoreId == id).Select(r => r.UserId).ToArray();

            //using (StoreContext db = new StoreContext())
            {
                var storeUsers = (from u in DbContext.UserProfiles where storeUserIds.Contains(u.UserId) select u).ToArray();
                ViewBag.Roles = DbContext.Roles.ToList();
                return View(storeUsers.ToList());
            }

        }
        // GET: /Stores/Details/5
        public ActionResult Settings(int id)
        {
            var store = this.StoreRepository.GetSingle(id);
            ViewBag.Store = store;
            var settings = this.SettingRepository.GetStoreSettings(id);
            return View(settings);
        }
        [HttpPost]
        public ActionResult SaveOrUpdateSetting(Setting setting)
        {
            if (ModelState.IsValid)
            {
                this.SettingRepository.Add(setting);
                this.SettingRepository.Save();
            }
            return RedirectToAction("Settings", new { id = setting.StoreId });
        }

    }
}
