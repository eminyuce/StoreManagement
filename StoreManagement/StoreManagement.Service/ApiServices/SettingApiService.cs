using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.ApiServices;

namespace StoreManagement.Service.ApiServices
{
    public class SettingApiService : BaseApiService, ISettingService
    {

        protected override string ApiControllerName { get { return "Settings"; } }
        public SettingApiService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<Setting> GetStoreSettings(int storeid)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreSettings?storeid={2}", WebServiceAddress, ApiControllerName, storeid);
            SetCache();
            return HttpRequestHelper.GetUrlResults<Setting>(url);
        }

        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {

            string url = string.Format("http://{0}/api/{1}/GetStoreSettingsFromCache?storeid={2}", WebServiceAddress, ApiControllerName, storeid);
            SetCache();
            Logger.Trace(String.Format("GetStoreSettingsFromCache {0} {1}", CacheMinute, IsCacheEnable));
            var items = HttpRequestHelper.GetUrlResults<Setting>(url);
            return items;
        }

        public Task<List<Setting>> GetStoreSettingsFromCacheAsync(int storeid)
        {

            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetStoreSettingsFromCacheAsync?storeid={2}", WebServiceAddress, ApiControllerName, storeid);
                return HttpRequestHelper.GetUrlResultsAsync<Setting>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreSettings?storeid={2}&type={3}", WebServiceAddress, ApiControllerName, storeid, type);
            SetCache();
            var items = HttpRequestHelper.GetUrlResults<Setting>(url);

            return items;
        }

        public Task<Setting> GetStoreSettingsByKey(int storeid, string key)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreSettingsByKey?storeid={2}&key={3}", WebServiceAddress, ApiControllerName, storeid, key);
            SetCache();
            var items = HttpRequestHelper.GetUrlResultAsync<Setting>(url);

            return items;
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
