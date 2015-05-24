using System;
using System.Collections.Generic;
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
            return PartialView("_StoresFilter", storeRepository.GetAll().ToList());
        }


        public PartialViewResult StoresDropDown(int storeId = 0)
        {
            ViewBag.StoreId = storeId;
            return PartialView("_StoresDropDown", storeRepository.GetAll().ToList());
        }
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
                        UserProfile user = dbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.UserName.ToLower());
                        userId = user.UserId;
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
            var store = this.storeRepository.GetSingle(id);
            ViewBag.Store = store;
            var storeUserIds = StoreUserRepository.FindBy(r => r.StoreId == id).Select(r => r.UserId).ToArray();

            //using (StoreContext db = new StoreContext())
            {
                var storeUsers = (from u in dbContext.UserProfiles where storeUserIds.Contains(u.UserId) select u).ToArray();
                ViewBag.Roles = dbContext.Roles.ToList();
                return View(storeUsers.ToList());
            }

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
