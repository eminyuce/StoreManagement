using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class SettingRepository : BaseRepository<Setting, int>, ISettingRepository
    {
        private readonly TypedObjectCache<List<Setting>> _settingStoreCache = new TypedObjectCache<List<Setting>>("SettingsCache");



        public SettingRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }



        public List<Setting> GetStoreSettings(int storeid)
        {
            var items = StoreDbContext.Settings.Where(r => r.StoreId == storeid).ToList();

            return items;
        }

        public void SaveSetting(int storeid, string key, string value, String type)
        {
            if (storeid > 0)
            {
                return;
            }

            List<Setting> resultSettings = GetStoreSettingsByType(storeid, "", key);
            if (!resultSettings.Any())
            {
                var setting = new Setting();
                setting.StoreId = storeid;
                setting.SettingKey = key;
                setting.SettingValue = value;
                setting.Type = type;
                setting.CreatedDate = DateTime.Now;
                setting.UpdatedDate = DateTime.Now;
                setting.State = true;
                setting.Ordering = -1;
                setting.Name = "";
                setting.Description = "";
                AddAsync(setting);

            }

        }

        public void SaveSetting()
        {
            MemoryCacheHelper.ClearCache("GetStoreSettingsFromCache-");
            this.Save();
        }


        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {
            String key = String.Format("GetStoreSettingsFromCache-{0}", storeid);
            _settingStoreCache.IsCacheEnable = this.IsCacheEnable;
            List<Setting> items = null;
            _settingStoreCache.TryGet(key, out items);
            if (items == null)
            {
                items = GetStoreSettings(storeid).Where(r => r.State).OrderBy(r => r.Ordering).ToList();
                _settingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));

            }
            return items;
        }

        public async Task<List<Setting>> GetStoreSettingsFromCacheAsync(int storeid)
        {
            try
            {
                Expression<Func<Setting, bool>> match = r2 => r2.StoreId == storeid && r2.State;
                var items = this.FindAllAsync(match, t => t.Ordering, OrderByType.Descending, null, null);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            Logger.Trace("GetStoreSettingsByType StoreId:" + storeid + " Type:" + type);
            return this.FindBy(r => r.StoreId == storeid
                && r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type, string search)
        {
            var items = this.FindBy(r => r.StoreId == storeid);

            if (!String.IsNullOrEmpty(type))
            {
                items = items.Where(r => r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                search = search.ToLower();
                items = items.Where(r => r.SettingKey.ToLower().Contains(search) || r.SettingValue.ToLower().Contains(search));

            }

            return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Name).ToList();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }


}
