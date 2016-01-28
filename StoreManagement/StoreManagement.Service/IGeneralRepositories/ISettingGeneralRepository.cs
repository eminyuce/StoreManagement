using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface ISettingGeneralRepository : IGeneralRepository
    {
        List<Setting> GetStoreSettings(int storeid);
        List<Setting> GetStoreSettingsFromCache(int storeid);
        Task<List<Setting>> GetStoreSettingsFromCacheAsync(int storeid);
        List<Setting> GetStoreSettingsByType(int storeid, string type);
        Task<Setting> GetStoreSettingsByKey(int storeid, String key);
    }
}
