using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService : IBaseService
    {
        [Inject]
        public IMessageGeneralRepository MessageRepository { set; get; }


        [Inject]
        public IStoreGeneralRepository StoreRepository { set; get; }

        [Inject]
        public IItemFileGeneralRepository ItemFileRepository { set; get; }

        [Inject]
        public ISettingGeneralRepository SettingRepository { set; get; }

        [Inject]
        public IFileManagerGeneralRepository FileManagerService { get; set; }

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

        protected bool CheckRequest(BaseEntity entity)
        {
            return entity.StoreId == MyStore.Id;
        }
    }
}
