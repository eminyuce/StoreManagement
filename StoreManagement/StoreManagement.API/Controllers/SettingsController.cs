using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;

namespace StoreManagement.API.Controllers
{
    public class SettingsController : BaseApiController, ISettingService
    {
        public List<Setting> GetStoreSettings(int storeid)
        {
            return SettingRepository.GetStoreSettings(storeid);
        }

        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {
            return SettingRepository.GetStoreSettingsFromCache(storeid);
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            return SettingRepository.GetStoreSettingsByType(storeid, type);
        }
    }
}