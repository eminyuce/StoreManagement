using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidHelpers;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Liquid.Constants;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using NLog;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Services;

namespace StoreManagement.Liquid.Controllers
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
        public IMessageService MessageService { set; get; }


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
        public ICommentService CommentService { set; get; }

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
        public IActivityService ActivityService { set; get; }

        [Inject]
        public IRetailerService RetailerService { set; get; }

        [Inject]
        public IProductService ProductService { set; get; }

        [Inject]
        public IProductAttributeService ProductAttributeService { set; get; }

        [Inject]
        public IProductAttributeRelationService ProductAttributeRelationService { set; get; }

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


        protected int StoreId { get; set; }
        protected String StoreName { get; set; }
        protected Store MyStore { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            GetStoreByDomain(requestContext);



        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            ViewData[StoreConstants.MetaTagKeywords] = GetSettingValue(StoreConstants.MetaTagKeywords, "");
            ViewData[StoreConstants.MetaTagDescription] = GetSettingValue(StoreConstants.MetaTagDescription, "");
            ViewData[StoreConstants.CanonicalUrl] = GetSettingValue(StoreConstants.CanonicalUrl, "");
            ViewData["StoreName"] = StoreName;

            SetStoreCache();



            base.OnActionExecuting(filterContext);
        }
         private static readonly TypedObjectCache<Store>
            StoreCache = new TypedObjectCache<Store>("StoreHelper");

        public Store GetStoreByDomain(HttpContextBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");
            String domainName = "FUELTECHNOLOGYAGE.COM";
            domainName = GeneralHelper.GetSiteDomain(request);
            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                String key = domainName;
                Store storeObj = new Store();
                storeObj = (Store) MemoryCache.Default.Get(key);
                if (storeObj == null)
                {
                    storeObj = StoreService.GetStoreByDomain(domainName);

                    CacheItemPolicy policy = null;

                    policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ProjectAppSettings.CacheLongSeconds);

                    MemoryCache.Default.Set(key, storeObj, policy);
                }
                return storeObj;

            }
            else
            {
                String defaultSiteDomain = ProjectAppSettings.GetWebConfigString("DefaultSiteDomain",
                                                                                 "login.seatechnologyjobs.com");
                BaseLogger.Trace("Default Site Domain is used." + defaultSiteDomain + " for " + domainName);
                return StoreService.GetStoreByDomain(defaultSiteDomain);
            }

        }

        private void GetStoreByDomain(RequestContext requestContext)
        {
            var storeObj = GetStoreByDomain(requestContext.HttpContext);
            this.StoreId = storeObj.Id;
            this.StoreName = storeObj.Name;
            this.MyStore = storeObj;

            if (StoreId == 0)
            {
                throw new Exception("Store cannot be NULL");
            }
        }

        protected bool IsCacheEnable { get; set; }
        protected void SetStoreCache()
        {
            if (StoreService == null)
            {
                BaseLogger.Trace("StoreService is null");
                return;
            }
            var isCacheEnable = StoreService.GetStoreCacheStatus(StoreId);
            this.IsCacheEnable = isCacheEnable;
            // Logger.Trace("StoreId =" + StoreId + " " + isCacheEnable);
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
            BaseLogger.Trace("Store:" + StoreId + " HttpNotFoundResult:" + statusDescription + " ");
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
        private readonly TypedObjectCache<List<FileManager>> _imagesStoreCache = new TypedObjectCache<List<FileManager>>("FileManagersCache");


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
        protected List<FileManager> GetStoreImages()
        {
            String key = String.Format("GetStoreImages-{0}", StoreId);
            _settingStoreCache.IsCacheEnable = true;
            List<FileManager> items = null;
            _imagesStoreCache.TryGet(key, out items);
            if (items == null)
            {
                var itemsAsyn = FileManagerService.GetImagesByStoreId(StoreId, true);

                _imagesStoreCache.Set(key, itemsAsyn, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));

            }
            return items;

        }

    }
}