using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
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
        private readonly TypedObjectCache<List<Setting>> _settingStoreCache= new TypedObjectCache<List<Setting>>("SettingsCache");



        public SettingRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }



        public List<Setting> GetStoreSettings(int storeid)
        {
            var items = StoreDbContext.Settings.Where(r => r.StoreId == storeid).ToList();

            return items;
        }
        public void SaveSetting()
        {
            ClearCache("GetStoreSettingsFromCache-");
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
                items = GetStoreSettings(storeid);
                _settingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));

            }
            return items;
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            Logger.Trace("GetStoreSettingsByType StoreId:" + storeid + " Type:" + type);
            return this.FindBy(r => r.StoreId == storeid
                && r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type, string search)
        {
            var items = this.FindBy(r => r.StoreId == storeid
                                         && r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                items = items.Where(r => r.SettingKey.ToLower().Contains(search.ToLower().Trim()));
            }

            return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }


    }


}
