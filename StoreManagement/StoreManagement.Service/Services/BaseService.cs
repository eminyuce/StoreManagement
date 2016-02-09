using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService : IBaseService
    {

        protected static readonly Logger BaseLogger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IMessageGeneralRepository MessageRepository { set; get; }


        [Inject]
        public IStoreGeneralRepository StoreRepository { set; get; }

        [Inject]
        public IItemFileGeneralRepository ItemFileRepository { set; get; }

        [Inject]
        public ISettingGeneralRepository SettingRepository { set; get; }

        [Inject]
        public IFileManagerGeneralRepository FileManagerRepository { get; set; }

        [Inject]
        public IContentFileGeneralRepository ContentFileRepository { set; get; }

        [Inject]
        public ICommentGeneralRepository CommentRepository { set; get; }

        [Inject]
        public IContentGeneralRepository ContentRepository { set; get; }

        [Inject]
        public ICategoryGeneralRepository CategoryRepository { set; get; }

        [Inject]
        public INavigationGeneralRepository NavigationRepository { set; get; }

        [Inject]
        public IPageDesignGeneralRepository PageDesignRepository { set; get; }

        [Inject]
        public IStoreUserGeneralRepository StoreUserRepository { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }


        [Inject]
        public IActivityGeneralRepository ActivityRepository { set; get; }

        [Inject]
        public IRetailerGeneralRepository RetailerRepository { set; get; }

        [Inject]
        public IProductGeneralRepository ProductRepository { set; get; }

        [Inject]
        public IProductAttributeGeneralRepository ProductAttributeRepository { set; get; }

        [Inject]
        public IProductAttributeRelationGeneralRepository ProductAttributeRelationRepository { set; get; }

        [Inject]
        public IProductFileGeneralRepository ProductFileRepository { set; get; }

        [Inject]
        public IProductCategoryGeneralRepository ProductCategoryRepository { set; get; }

        [Inject]
        public IBrandGeneralRepository BrandRepository { set; get; }

        [Inject]
        public ILocationGeneralRepository LocationRepository { set; get; }

        [Inject]
        public IContactGeneralRepository ContactRepository { set; get; }


        [Inject]
        public ILabelGeneralRepository LabelRepository { set; get; }


        public Store MyStore { get; set; }
        public int StoreId { get { return MyStore.Id; } }

        private readonly TypedObjectCache<List<Setting>> _settingStoreCache = new TypedObjectCache<List<Setting>>("SettingsCache");
        protected List<Setting> GetStoreSettings()
        {
            String key = String.Format("GetStoreSettingsFromCacheAsync-{0}", StoreId);
            _settingStoreCache.IsCacheEnable = true;
            List<Setting> items = null;
            _settingStoreCache.TryGet(key, out items);
            if (items == null)
            {
                var itemsAsyn = SettingRepository.GetStoreSettingsFromCache(StoreId);

                items = itemsAsyn;
                _settingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));

            }
            return items;

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

                return item != null ? item.SettingValue : "";
            }
            catch (Exception ex)
            {

                BaseLogger.Error(ex, "Store= " + StoreId + " Key=" + key, key);
                return "";
            }
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == MyStore.Id;
        }

        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
    }
}
