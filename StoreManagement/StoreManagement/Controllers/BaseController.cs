using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using Ninject;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Helper;
using StoreManagement.Models;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger BaseLogger = LogManager.GetCurrentClassLogger();



        [Inject]
        public IActivityHelper ActivityHelper { set; get; }

        [Inject]
        public ICommentHelper CommentHelper { set; get; }


        [Inject]
        public IRetailerHelper RetailerHelper { set; get; }

        [Inject]
        public IContactHelper ContactHelper { set; get; }

        [Inject]
        public ILocationHelper LocationHelper { set; get; }

        [Inject]
        public IBrandHelper BrandHelper { set; get; }

        [Inject]
        public IContentHelper ContentHelper { set; get; }

        [Inject]
        public IProductHelper ProductHelper { set; get; }

        [Inject]
        public IPagingHelper PagingHelper { set; get; }

        [Inject]
        public ILabelHelper LabelHelper { set; get; }

        [Inject]
        public IPhotoGalleryHelper PhotoGalleryHelper { set; get; }

        [Inject]
        public IProductCategoryHelper ProductCategoryHelper { set; get; }

        [Inject]
        public ICategoryHelper CategoryHelper { set; get; }

        [Inject]
        public INavigationHelper NavigationHelper { set; get; }

        [Inject]
        public IHomePageHelper HomePageHelper { set; get; }

        [Inject]
        public ISiteMapService SiteMapService { set; get; }

        [Inject]
        public IProductService ProductService { set; get; }

        [Inject]
        public IMessageGeneralRepository MessageService { set; get; }


        [Inject]
        public IStoreGeneralRepository StoreService { set; get; }

        [Inject]
        public IFileManagerService FileManagerService { set; get; }
        
        [Inject]
        public ISettingGeneralRepository SettingService { set; get; }
        
        [Inject]
        public IContentFileGeneralRepository ContentFileService { set; get; }

        [Inject]
        public ICommentGeneralRepository CommentService { set; get; }

        [Inject]
        public IContentGeneralRepository ContentService { set; get; }

        [Inject]
        public ICategoryGeneralRepository CategoryService { set; get; }

      
        [Inject]
        public IPageDesignGeneralRepository PageDesignService { set; get; }
 

        [Inject]
        public IActivityGeneralRepository ActivityService { set; get; }

        [Inject]
        public IProductGeneralRepository ProductRepository { set; get; }

        [Inject]
        public IProductAttributeGeneralRepository ProductAttributeService { set; get; }

        [Inject]
        public IProductAttributeRelationGeneralRepository ProductAttributeRelationService { set; get; }

        [Inject]
        public IProductFileGeneralRepository ProductFileService { set; get; }

        [Inject]
        public IProductCategoryGeneralRepository ProductCategoryService { set; get; }

        [Inject]
        public IBrandGeneralRepository BrandService { set; get; }

        [Inject]
        public ILocationGeneralRepository LocationService { set; get; }

        [Inject]
        public IContactGeneralRepository ContactService { set; get; }


        [Inject]
        public ILabelGeneralRepository LabelService { set; get; }


        [Inject]
        public INavigationService NavigationService { set; get; }



        [Inject]
        public IEmailSender EmailSender { set; get; }

        [Inject]
        public ILocationService LocationService2 { set; get; }

        [Inject]
        public IBrandService BrandService2 { set; get; }

        [Inject]
        public IContentService ContentService2 { set; get; }

        [Inject]
        public IContactService ContactService2 { set; get; }

        [Inject]
        public INavigationService NavigationService2 { set; get; }

        [Inject]
        public IStoreService StoreService2 { set; get; }

        [Inject]
        public ICategoryService CategoryService2 { set; get; }

        [Inject]
        public IRetailerService RetailerService2 { set; get; }

        [Inject]
        public IProductCategoryService ProductCategoryService2 { set; get; }

        [Inject]
        public IProductService ProductService2 { set; get; }

    


        protected Store MyStore { set; get; }
        protected int StoreId { get; set; }
        protected String StoreName { get; set; }



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
            this.MyStore = store;
            this.StoreId = store.Id;
            this.StoreName = store.Name;
            SetServiceStore(store);

            if (store == null)
            {
                throw new Exception("Store cannot be NULL");
            }
        }

        private void SetServiceStore(Store store)
        {
            ProductService2.MyStore = store;
            ContentService2.MyStore = store;
            LocationService2.MyStore = store;
            BrandService2.MyStore = store;
            RetailerService2.MyStore = store;
            ContactService2.MyStore = store;
            NavigationService2.MyStore = store;
            StoreService2.MyStore = store;
            CategoryService2.MyStore = store;
            ProductCategoryService2.MyStore = store;
        }

        protected new HttpNotFoundResult HttpNotFound(string statusDescription = null)
        {
            return new HttpNotFoundResult(statusDescription);
        }
        protected bool IsModulActive(String controllerName)
        {
            return NavigationService.IsModulActive(controllerName);
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == MyStore.Id;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            ViewData[StoreConstants.MetaTagKeywords] = GetSettingValue(StoreConstants.MetaTagKeywords, "");
            ViewData[StoreConstants.MetaTagDescription] = GetSettingValue(StoreConstants.MetaTagDescription, "");
            ViewData[StoreConstants.CanonicalUrl] = GetSettingValue(StoreConstants.CanonicalUrl, "");
            ViewData["StoreName"] = MyStore.Name;

            SetStoreCache();


            base.OnActionExecuting(filterContext);
        }
        protected bool IsCacheEnable { get; set; }
        protected void SetStoreCache()
        {
            if (StoreService == null)
            {
                BaseLogger.Trace("StoreService is null");
                return;
            }
            //var isCacheEnable = StoreService.GetStoreCacheStatus(StoreId);
            //this.IsCacheEnable = isCacheEnable;
            //// Logger.Trace("StoreId =" + StoreId + " " + isCacheEnable);
            //SettingService.IsCacheEnable = isCacheEnable;
            //SettingService.CacheMinute = GetSettingValueInt("SettingService_CacheMinute", 200);


            //NavigationService.IsCacheEnable = isCacheEnable;
            //NavigationService.CacheMinute = GetSettingValueInt("NavigationService_CacheMinute", 200);

            //ProductCategoryService.IsCacheEnable = isCacheEnable;
            //ProductCategoryService.CacheMinute = GetSettingValueInt("ProductCategoryService_CacheMinute", 200);

            //ProductFileService.IsCacheEnable = isCacheEnable;
            //ProductFileService.CacheMinute = GetSettingValueInt("ProductFileService_CacheMinute", 200);

            //ProductService.IsCacheEnable = isCacheEnable;
            //ProductService.CacheMinute = GetSettingValueInt("ProductService_CacheMinute", 200);


            //StoreUserService.IsCacheEnable = isCacheEnable;
            //StoreUserService.CacheMinute = GetSettingValueInt("StoreUserService_CacheMinute", 200);

            //PageDesignService.IsCacheEnable = isCacheEnable;
            //PageDesignService.CacheMinute = GetSettingValueInt("PageDesignService_CacheMinute", 200);

            //CategoryService.IsCacheEnable = isCacheEnable;
            //CategoryService.CacheMinute = GetSettingValueInt("CategoryService_CacheMinute", 200);

            //ContentService.IsCacheEnable = isCacheEnable;
            //ContentService.CacheMinute = GetSettingValueInt("ContentService_CacheMinute", 200);

            //ContentFileService.IsCacheEnable = isCacheEnable;
            //ContentFileService.CacheMinute = GetSettingValueInt("ContentFileService_CacheMinute", 200);

            //FileManagerService.IsCacheEnable = isCacheEnable;
            //FileManagerService.CacheMinute = GetSettingValueInt("FileManagerService_CacheMinute", 200);


            //BrandService.IsCacheEnable = isCacheEnable;
            //BrandService.CacheMinute = GetSettingValueInt("BrandService_CacheMinute", 200);

            //LocationService.IsCacheEnable = isCacheEnable;
            //LocationService.CacheMinute = GetSettingValueInt("LocationService_CacheMinute", 200);

            //ContactService.IsCacheEnable = isCacheEnable;
            //ContactService.CacheMinute = GetSettingValueInt("ContactService_CacheMinute", 200);

            //StoreService.IsCacheEnable = isCacheEnable;
            //StoreService.CacheMinute = GetSettingValueInt("StoreService_CacheMinute", 200);
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
                BaseLogger.Trace("Store Default Setting= " + StoreId + " Key=" + key + " defaultValue=" + defaultValue);
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
                if (StoreId == 0)
                {
                    return "";
                }
                var item = GetStoreSettings().FirstOrDefault(r => r.SettingKey.RemoveTabNewLines().Equals(key.RemoveTabNewLines(), StringComparison.InvariantCultureIgnoreCase));

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

                BaseLogger.Error(ex, "Store= " + StoreId + " Key=" + key, key);

                return "";
            }
        }
        private readonly TypedObjectCache<List<Setting>> _settingStoreCache = new TypedObjectCache<List<Setting>>("SettingsCache");

        protected List<Setting> GetStoreSettings()
        {
            String key = String.Format("GetStoreSettingsFromCacheAsync-{0}", StoreId);
            _settingStoreCache.IsCacheEnable = true;
            List<Setting> items = null;
            _settingStoreCache.TryGet(key, out items);
            if (items == null)
            {
                var itemsAsyn = SettingService.GetStoreSettingsFromCache(StoreId);

                items = itemsAsyn;
                _settingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));

            }
            return items;

        }

    }
}