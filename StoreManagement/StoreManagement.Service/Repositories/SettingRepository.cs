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
        private static readonly TypedObjectCache<List<Setting>> SettingStoreCache
            = new TypedObjectCache<List<Setting>>("categoryCache");


        public SettingRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Setting> GetStoreSettings(int storeid)
        {
            var items = StoreDbContext.Settings.Where(r => r.StoreId == storeid).ToList();

            return items;
        }

        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {
            String key = String.Format("Content-{0}", storeid);
            List<Setting> items = null;
            SettingStoreCache.TryGet(key, out items);
            if (items == null)
            {
                items = GetStoreSettings(storeid);
                SettingStoreCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Setting_CacheAbsoluteExpiration_Minute", 10)));
            }
            return items;
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            Logger.Trace("GetStoreSettingsByType StoreId:" + storeid + " Type:" + type);
            return  this.FindBy(r => r.StoreId == storeid 
                &&  r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }


}
