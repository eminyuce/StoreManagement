using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class StoreUsersController : UsersController
    {
        public override ActionResult Users(int storeId = 0, String search = "")
        {
            var storeUserIds = StoreUserRepository.GetAll().Select(r => r.UserId).ToList();

            var storeUsers = (from u in DbContext.UserProfiles where !storeUserIds.Contains(u.UserId) select u).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                storeUsers =
                    storeUsers.Where(r => r.UserName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            ViewBag.Roles = DbContext.Roles.ToList();
            return View(storeUsers.ToList());
        }
        public ActionResult Index(String search="")
        {
            return this.Users(0, search);
        }
        public override ActionResult DeleteStoreUser(string userName = "")
        {
            DeleteUser(userName);
            return RedirectToAction("Index","StoreUsers");
        }
        public override ActionResult SaveOrEditStoreUser(int storeId = 0, int userId = 0)
        {
            return base.SaveOrEditStoreUser(storeId, userId);
        }
        public override ActionResult SaveOrEditStoreUser(int storeId, Data.Entities.LoginModel userName, string roleName = "")
        {
            base.SaveOrEditStoreUser(storeId, userName, roleName);
            return RedirectToAction("Index", "StoreUsers");
        }
	}
}