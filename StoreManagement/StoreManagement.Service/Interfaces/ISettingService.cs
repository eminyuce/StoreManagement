using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface ISettingService : IService
    {
        List<Setting> GetStoreSettings(int storeid);
        List<Setting> GetStoreSettingsFromCache(int storeid);
        List<Setting> GetStoreSettingsByType(int storeid, string type);
    }
}
