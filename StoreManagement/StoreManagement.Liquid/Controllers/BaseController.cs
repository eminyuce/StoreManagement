using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;
using NLog;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IStoreService StoreService { set; get; }
        [Inject]
        public ISettingService SettingService { set; get; }


        [Inject]
        public IFileManagerService FileManagerService { get; set; }

        [Inject]
        public IContentFileService ContentFileService { set; get; }

        [Inject]
        public IContentService ContentService { set; get; }

        [Inject]
        public ICategoryService CategoryService { set; get; }

        [Inject]
        public INavigationService NavigationService { set; get; }

        [Inject]
        public IPageDesignService PageDesignService { set; get; }

        [Inject]
        public IStoreUserService StoreUserService { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }


        [Inject]
        public IProductService ProductService { set; get; }

        [Inject]
        public IProductFileService ProductFileService { set; get; }

        [Inject]
        public IProductCategoryService ProductCategoryService { set; get; }

        protected Store Store { set; get; }



        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            GetStoreByDomain(requestContext);

            
            ViewBag.MetaDescription = GetSettingValue(StoreConstants.MetaTagDescription);
            ViewBag.MetaKeywords = GetSettingValue(StoreConstants.MetaTagKeywords);

        }
        private void GetStoreByDomain(RequestContext requestContext)
        {
            var sh = new StoreHelper();
            var store = sh.GetStoreByDomain(StoreService, requestContext.HttpContext.Request);
            this.Store = store;
            if (store == null)
            {
                throw new Exception("Store cannot be NULL");
            }
        }


        protected new HttpNotFoundResult HttpNotFound(string statusDescription = null)
        {
            Logger.Trace("Store:" + Store.Name + " HttpNotFoundResult:" + statusDescription + " ");
            return new HttpNotFoundResult(statusDescription);
        }

        protected bool IsModulActive(String controllerName)
        {
            var navigations = NavigationService.GetStoreActiveNavigations(Store.Id);
            var item = navigations.Any(r => r.ControllerName.ToLower().StartsWith(controllerName.ToLower()));
            return item;
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == Store.Id;
        }
     
     

        protected bool GetSettingValueBool(String key, bool defaultValue)
        {
            String d = defaultValue ? bool.TrueString : bool.FalseString;
            return GetSettingValue(key, d).ToBool();
        }
        protected int GetSettingValueInt(String key, int defaultValue)
        {
            String d = defaultValue + "";
            return GetSettingValue(key, d).ToInt();
        }
        protected String GetSettingValue(String key, String defaultValue)
        {
            var value = GetSettingValue(key);
            if (String.IsNullOrEmpty(value))
            {
                Logger.Trace("Store Default Setting= " + Store.Domain + " Key=" + key + " defaultValue=" + defaultValue);
                return ProjectAppSettings.GetWebConfigString(key, defaultValue);
            }
            else
            {
                return value;
            }
        }
        protected String GetSettingValue(String key)
        {
            try
            {
                if (Store == null)
                {
                    return "";
                }
                var item = StoreSettings.FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                return item != null ? item.SettingValue : "";
            }
            catch (Exception ex)
            {
                if (Store != null)
                {
                    Logger.Error("Store= " + Store.Domain + " Key=" + key, ex);
                }
                return "";
            }
        }


        protected List<Setting> StoreSettings
        {
            get { return SettingService.GetStoreSettings(Store.Id); }
        } 

    }
}