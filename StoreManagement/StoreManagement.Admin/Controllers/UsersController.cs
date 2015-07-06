using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StoreManagement.Data.Entities;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public abstract class UsersController : BaseController
    {

        // GET: /Stores/Details/5
        public virtual ActionResult Users(int storeId, String search = "")
        {
            storeId = GetStoreId(storeId);
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


        public virtual ActionResult DeleteStoreUser(String userName = "")
        {
            int storeId = GetStoreId(0);
            try
            {
                DeleteUser(userName);


                return RedirectToAction("Index");
            }
            catch
            {
                return View(userName);
            }
        }

        protected void DeleteUser(string userName)
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
            ((SimpleMembershipProvider) Membership.Provider).DeleteAccount(userName);
                // deletes record from webpages_Membership table
            ((SimpleMembershipProvider) Membership.Provider).DeleteUser(userName, true);
                // deletes record from UserProfile table
        }


        public ActionResult StoreUserDetail(int storeId = 0, int userId = 0)
        {
            UserProfile storeUser = DbContext.UserProfiles.FirstOrDefault(r => r.UserId == userId);
            var store = StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;
            return View(storeUser);
        }

        public virtual ActionResult SaveOrEditStoreUser(int storeId = 0, int userId = 0)
        {
            storeId = GetStoreId(storeId);
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
        public virtual ActionResult SaveOrEditStoreUser(int storeId, LoginModel userName, String roleName = "")
        {
            storeId = GetStoreId(storeId);
            if (String.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError("UserName", "SELECT A ROLE PLEASE");

            }
            var store = this.StoreRepository.GetSingle(storeId);
            ViewBag.Store = store;

            ViewBag.Roles = DbContext.Roles.ToList();
            bool isSuperAdmin = false;

            try
            {
                isSuperAdmin = User.Identity.IsAuthenticated && Roles.GetRolesForUser(User.Identity.Name).Contains("SuperAdmin");
            }
            catch (Exception ex)
            {

            }
    
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


            return RedirectToAction(isSuperAdmin ? "Users" : "Index", new { storeId = storeId });
        }
    }
}