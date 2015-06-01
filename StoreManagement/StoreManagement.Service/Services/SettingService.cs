using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Services;

namespace StoreManagement.Service.Services
{
    public class SettingService : BaseService, ISettingService
    {
        public SettingService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        public List<Setting> GetStoreSettings(int storeid)
        {
            throw new NotImplementedException();
        }

        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {
            throw new NotImplementedException();
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            throw new NotImplementedException();
        }
    }
}
