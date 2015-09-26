using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
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

        public async Task<List<Setting>> GetStoreSettingsFromCacheAsync(int storeid)
        {
            return await SettingRepository.GetStoreSettingsFromCacheAsync(storeid);
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
            if (ModelState.IsValid)
            {

               Task.Factory.StartNew(() => SettingRepository.SaveSetting(value.StoreId, value.SettingKey, value.SettingValue,
                                                                                     "StoreSettings"));

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, value);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
                return response;

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
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