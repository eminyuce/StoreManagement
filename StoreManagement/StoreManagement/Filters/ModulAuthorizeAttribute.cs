using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Helper;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Filters
{
    public class ModulAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {

        public IStoreGeneralRepository StoreService { set; get; }


        public INavigationGeneralRepository NavigationService { set; get; }

        public ModulAuthorizeAttribute(IStoreGeneralRepository storeService, INavigationGeneralRepository navigationService)
        {
            this.StoreService = storeService;
            this.NavigationService = navigationService;
        }


        protected bool IsModulActive(String controllerName, int storeId)
        {
            return NavigationService.GetStoreActiveNavigations(storeId).Any(r => r.ControllerName.StartsWith(controllerName.ToLower()));
        }

        // This method must be thread-safe since it is called by the thread-safe OnCacheAuthorization() method.
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var sh = new StoreHelper();
            var store = sh.GetStoreByDomain(StoreService, filterContext.Controller.ControllerContext.RequestContext.HttpContext.Request);
            var controller = filterContext.RouteData.Values["controller"].ToStr();
            if (!IsModulActive(controller, store.Id))
            {
                filterContext.Result = new HttpUnauthorizedResult("NO ACCESS TO THE PAGE");
            }
            
        }

    }
}