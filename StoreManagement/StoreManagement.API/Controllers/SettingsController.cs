using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;

namespace StoreManagement.API.Controllers
{
    public class SettingsController : BaseApiController<Setting>, ISettingService
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
 

        public override IEnumerable<Setting> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Setting Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Setting value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Setting value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}