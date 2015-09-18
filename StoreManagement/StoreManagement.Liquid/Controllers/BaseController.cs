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
using StoreManagement.Liquid.Constants;
using StoreManagement.Liquid.Helper;
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using NLog;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Services;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IActivityHelper ActivityHelper { set; get; }

        [Inject]
        public ICommentHelper CommentHelper { set; get; }

        [Inject]
        public IStoreHelper StoreHelper { set; get; }

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
        public INavigationHelper NavigationHelper { set; get; }

        [Inject]
        public IHomePageHelper HomePageHelper { set; get; }


        [Inject]
        public IEmailSender EmailSender { set; get; }


        
        public IStoreService StoreService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new StoreService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new StoreRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public IItemFileService ItemFileService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ItemFileService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ItemFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public ISettingService SettingService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new SettingService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new SettingRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public IFileManagerService FileManagerService
        {
            get
            {
                if (ProjectAppSettings.IsApiService)
                {
                    return new FileManagerService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new FileManagerRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public IContentFileService ContentFileService
        {
            get
            {
                if (ProjectAppSettings.IsApiService)
                {
                    return new ContentFileService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ContentFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public IContentService ContentService
        {
            get
            {
                if (ProjectAppSettings.IsApiService)
                {
                    return new ContentService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ContentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public ICategoryService CategoryService
        {
            get
            {
                if (ProjectAppSettings.IsApiService)
                {
                    return new CategoryService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new CategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public INavigationService NavigationService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {
                    return new NavigationService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new NavigationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public IPageDesignService PageDesignService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new PageDesignService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new PageDesignRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public IStoreUserService StoreUserService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new StoreUserService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new StoreUserRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public ICommentService CommentService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new CommentService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new CommentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }




        public IActivityService ActivityService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ActivityService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ActivityRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }



        public IProductService ProductService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ProductService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ProductRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public IProductFileService ProductFileService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ProductFileService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ProductFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        public IProductCategoryService ProductCategoryService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ProductCategoryService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ProductCategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public IBrandService BrandService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new BrandService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new BrandRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public ILocationService LocationService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new LocationService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new LocationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

        
        public IContactService ContactService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new ContactService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new ContactRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }


        public ILabelService LabelService
        {
            get
            {

                if (ProjectAppSettings.IsApiService)
                {

                    return new LabelService(ProjectAppSettings.WebServiceAddress);
                }
                else
                {
                    return new LabelRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }
        }

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
            var store = StoreHelper.GetStoreIdByDomain(StoreService, requestContext.HttpContext);
            this.StoreId = store;


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
                Logger.Trace("StoreService is null");
                return;
            }
            var isCacheEnable = StoreService.GetStoreCacheStatus(StoreId);
            this.IsCacheEnable = isCacheEnable;
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

                Logger.Error(ex, "Store= " + StoreId + " Key=" + key, key);

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