using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreManagement.Data.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.Result == null || (filterContext.Result.GetType() != typeof(HttpUnauthorizedResult)
                || !filterContext.HttpContext.Request.IsAjaxRequest()))
                return;
 
            var redirectToUrl = "Account/Login?returnUrl=" + filterContext.HttpContext.Request.UrlReferrer.PathAndQuery;

            filterContext.Result = new JavaScriptResult() { Script = "window.location = '" + redirectToUrl + "'" };

        }
    }
}
