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
        public IItemFileService ItemFileService { set; get; }

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

        [Inject]
        public IBrandService BrandService { set; get; }

        [Inject]
        public ILocationService LocationService { set; get; }

        [Inject]
        public IContactService ContactService { set; get; }


        [Inject]
        public ILabelService LabelService { set; get; }

        // protected Store Store { set; get; }
        protected int StoreId { get; set; }
       
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            GetStoreByDomain(requestContext);


            ViewBag.MetaDescription = GetSettingValue(StoreConstants.MetaTagDescription);
            ViewBag.MetaKeywords = GetSettingValue(StoreConstants.MetaTagKeywords);

        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetStoreCache();
            base.OnActionExecuting(filterContext);
        }
       
        private void GetStoreByDomain(RequestContext requestContext)
        {
            var sh = new StoreHelper();
            var store = sh.GetStoreIdByDomain(StoreService, requestContext.HttpContext);
            this.StoreId = store;
           
   
            if (StoreId  == 0)
            {
                throw new Exception("Store cannot be NULL");
            }
        }

   
        protected void SetStoreCache()
        {
            if (StoreService == null)
            {
                Logger.Trace("StoreService is null");
                return;
            }
            var isCacheEnable = StoreService.GetStoreCacheStatus(StoreId);
            Logger.Trace("StoreId =" + StoreId + " " + isCacheEnable);
            SettingService.IsCacheEnable = isCacheEnable;
            SettingService.CacheMinute = GetSettingValueInt("SettingService_CacheMinute", 200);


            NavigationService.IsCacheEnable = isCacheEnable;
            NavigationService.CacheMinute = GetSettingValueInt("NavigationService_CacheMinute", 200);

            ProductCategoryService.IsCacheEnable = isCacheEnable;
            ProductCategoryService.CacheMinute = GetSettingValueInt("ProductCategoryService_CacheMinute", 200);

            ProductFileService.IsCacheEnable = isCacheEnable;
            ProductFileService.CacheMinute = GetSettingValueInt("ProductFileService_CacheMinute", 200);

            ProductService.IsCacheEnable = isCacheEnable;
            ProductService.CacheMinute = GetSettingValueInt("ProductService_CacheMinute", 200);


            StoreUserService.IsCacheEnable = isCacheEnable;
            StoreUserService.CacheMinute = GetSettingValueInt("StoreUserService_CacheMinute", 200);

            PageDesignService.IsCacheEnable = isCacheEnable;
            PageDesignService.CacheMinute = GetSettingValueInt("PageDesignService_CacheMinute", 200);

            CategoryService.IsCacheEnable = isCacheEnable;
            CategoryService.CacheMinute = GetSettingValueInt("CategoryService_CacheMinute", 200);

            ContentService.IsCacheEnable = isCacheEnable;
            ContentService.CacheMinute = GetSettingValueInt("ContentService_CacheMinute", 200);

            ContentFileService.IsCacheEnable = isCacheEnable;
            ContentFileService.CacheMinute = GetSettingValueInt("ContentFileService_CacheMinute", 200);

            FileManagerService.IsCacheEnable = isCacheEnable;
            FileManagerService.CacheMinute = GetSettingValueInt("FileManagerService_CacheMinute", 200);


            BrandService.IsCacheEnable = isCacheEnable;
            BrandService.CacheMinute = GetSettingValueInt("BrandService_CacheMinute", 200);

            LocationService.IsCacheEnable = isCacheEnable;
            LocationService.CacheMinute = GetSettingValueInt("LocationService_CacheMinute", 200);

            ContactService.IsCacheEnable = isCacheEnable;
            ContactService.CacheMinute = GetSettingValueInt("ContactService_CacheMinute", 200);

            StoreService.IsCacheEnable = isCacheEnable;
            StoreService.CacheMinute = GetSettingValueInt("StoreService_CacheMinute", 200);
        }


        protected new HttpNotFoundResult HttpNotFound(string statusDescription = null)
        {
            Logger.Trace("Store:" + StoreId + " HttpNotFoundResult:" + statusDescription + " ");
            return new HttpNotFoundResult(statusDescription);
        }

        protected bool IsModulActive(String controllerName)
        {
            var navigations = NavigationService.GetStoreActiveNavigations(StoreId);
            var item = navigations.Any(r => r.ControllerName.ToLower().StartsWith(controllerName.ToLower()));
            return item;
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == StoreId;
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
                Logger.Trace("Store Default Setting= " + StoreId + " Key=" + key + " defaultValue=" + defaultValue);
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
                if (StoreId > 0)
                {
                    return "";
                }
                var item = GetStoreSettings().FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                if (item != null)
                {
                    return item.SettingValue;
                }
                else
                {
                    return "";
                }

                
            }
            catch (Exception ex)
            {
                
                    Logger.Error(ex, "Store= " + StoreId + " Key=" + key, key);
                
                return "";
            }
        }


        protected List<Setting> GetStoreSettings()
        {

            var items = SettingService.GetStoreSettingsFromCache(StoreId);
            return items;

        }

    }
}