using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using RazorEngine;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;
using StoreManagement.Admin.Filters;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {

            var regexUtil = new RegexUtilities();
            if (!regexUtil.IsValidEmail(model.UserName))
            {
                ViewBag.ReturnUrl = returnUrl;
                ModelState.AddModelError("UserName", "Invalid Email Address");
                return View(model);
            }


            //validate the captcha through the session variable stored from GetCaptcha
            if (Session["CaptchaStoreLogin"] == null || Session["CaptchaStoreLogin"].ToString() != model.Captcha)
            {
                ModelState.AddModelError("Captcha", "Wrong sum, please try again.");
                return View(model);
            }
            else
            {

                model.RememberMe = true;
                if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    LoginUserCredential(model.UserName, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }

                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }
        }

        private void LoginUserCredential(String userName, bool rememberMe)
        {

            UserProfile user = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());
            user.LastLoginDate = DateTime.Now;
            DbContext.SaveChanges();

            var isSuper = Roles.GetRolesForUser(userName).Contains("SuperAdmin");
            string userData = "";
            if (!isSuper)
            {
               
                var s = StoreRepository.GetStoreByUserName(userName);
                userData = s.Id.ToStr();
            }
            else
            {
                userData = isSuper.ToString();
            }
            
            SetAuthCookie(userName, rememberMe, userData);
        }
        public int SetAuthCookie(string userName, bool rememberMe, String userData)
        {
            /// In order to pickup the settings from config, we create a default cookie and use its values to create a 
            /// new one.
           
            String cookiestr;
            HttpCookie ck;
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now,
                DateTime.Now.AddMinutes(30), rememberMe, userData);

            cookiestr = FormsAuthentication.Encrypt(ticket);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName.ToString(), cookiestr);

            if (rememberMe)
            {
                ck.Expires = ticket.Expiration;
            }
            ck.Path = FormsAuthentication.FormsCookiePath;

            Response.Cookies.Add(ck);


            return cookiestr.Length;
        }
        public ActionResult NoStoreFound()
        {

            DeleteAuthCookie();

            WebSecurity.Logout();
            return View();
        }

        private void DeleteAuthCookie()
        {
            FormsAuthentication.SignOut();
            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            DeleteAuthCookie();
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Dashboard");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                //using (UsersContext db = new UsersContext())
                {
                    UserProfile user = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        DbContext.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        DbContext.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            //if (Url.IsLocalUrl(returnUrl) && !returnUrl.ToLower().Contains("account/login") && !returnUrl.ToLower().Contains("home/index"))
            //{
            //    return Redirect(returnUrl);
            //}
            //else
            //{

            //}

            return RedirectToAction("Index", "Dashboard");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string userName, String captcha = "")
        {
            //validate the captcha through the session variable stored from GetCaptcha
            if (Session["CaptchaForgotPassword"] == null || Session["CaptchaForgotPassword"].ToString() != captcha)
            {
                TempData["Message"] = "Wrong sum, please try again.";
                return View();
            }
            else
            {

                //check user existance
                var user = Membership.GetUser(userName);
                if (user == null)
                {
                    TempData["Message"] = "User Not exist.";
                }
                else
                {
                    String token = "";
                    try
                    {
                        var userId = WebSecurity.GetUserId(userName);
                        bool any = (from j in DbContext.WebpagesMemberships
                                    where (j.UserId == userId)
                                    select j).Any();

                        if (any)
                        {
                            //generate password token
                            token = WebSecurity.GeneratePasswordResetToken(userName);
                        }
                        else
                        {
                            WebSecurity.CreateAccount(userName, GeneralHelper.GenerateRandomPassword(6));
                            //generate password token
                            token = WebSecurity.GeneratePasswordResetToken(userName);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Exception is occured in ForgotPassword-Account controller", ex);
                    }

                    //create url with above token
                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { un = userName, rt = token }, "http");
                    var resetLink = "<a href='" + resetPasswordUrl + "'>Reset Password</a>";
                    //  var mailTemplate = MailTemplateRepository.GetMailTemplatesByTemplateName("ForgotPassword");
                    //  mailTemplate.Body = MailHelper.GetForgotPasswordMailTemplate(mailTemplate.Body, UserName, resetLink, resetPasswordUrl);

                    try
                    {
                        String forgotPasswordEmailTemplate = "";
                        forgotPasswordEmailTemplate = System.IO.File.ReadAllText(Server.MapPath(@"~\Views\Shared\DisplayTemplates\ForgotPasswordEmail.cshtml"));
                        var model = new { Email = userName, Link = resetLink, Url = resetPasswordUrl };

                        string subject = "Store Management Reset Password";
                        string body = Razor.Parse(forgotPasswordEmailTemplate, model);
                        string fromAddress = "eminyuce@gmail.com";
                        string fromName = "eminyuce@gmail.com";
                        string toAddress = userName;
                        string toName = userName;

                        try
                        {
                            EmailSender.SendEmail(EmailAccount.GetAdminEmailAccount(), subject, body, fromAddress, fromName, toAddress, toName);
                            TempData["Message"] =
                                "Password Reset link is sent to your acccount.Please click the link in your mail to generate random password.";

                        }
                        catch (Exception ex)
                        {
                            TempData["Message"] = "Error:" + ex.Message;
                            Logger.ErrorException("Error:" + ex.Message, ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        String m = "Error occured while sending email:" + ex.Message;
                        TempData["Message"] = m;
                        Logger.Error(ex.ToString(), ex);
                    }
                }

                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            //TODO: Check the un and rt matching and then perform following
            //get userid of received username
            UserProfile dbUser = DbContext.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == un.ToLower());
            //check userid and token matches
            bool any = (from j in DbContext.WebpagesMemberships
                        where (j.UserId == dbUser.UserId)
                        && (j.PasswordVerificationToken == rt)
                        //&& (j.PasswordVerificationTokenExpirationDate < DateTime.Now)
                        select j).Any();

            if (any == true)
            {
                //generate random password
                string newpassword = GeneralHelper.GenerateRandomPassword(6);
                //reset password
                bool response = WebSecurity.ResetPassword(rt, newpassword);

                var token = WebSecurity.GeneratePasswordResetToken(dbUser.UserName);
                ViewBag.ChangePassToken = token;
                ViewBag.UserName = dbUser.UserName;
                ViewBag.OldPassword = newpassword;
                TempData["Message"] = "Your New Password Is " + newpassword + ".Please change your password.";

            }
            else
            {
                TempData["Message"] = "Username and token not maching.";
            }




            return View();


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ResetPassword(LocalPasswordModel model, string un, string rt)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(un));
            ViewBag.HasLocalPassword = hasLocalAccount;


            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded = false;
                    try
                    {
                        //Reset password
                        changePasswordSucceeded = WebSecurity.ResetPassword(rt, model.NewPassword);

                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {

                        LoginUserCredential(un, true);
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The new password is invalid.");
                    }
                }
            }

            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.UserName = un;
            ViewBag.ChangePassToken = rt;
            ViewBag.OldPassword = model.OldPassword;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
