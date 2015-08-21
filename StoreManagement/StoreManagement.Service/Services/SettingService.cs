using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Services;

namespace StoreManagement.Service.Services
{
    public class SettingService : BaseService, ISettingService
    {

        protected override string ApiControllerName { get { return "Settings"; } }
        public SettingService(string webServiceAddress)
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

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreSettings?storeid={2}&type={3}", WebServiceAddress, ApiControllerName, storeid, type);
            SetCache();
            var items = HttpRequestHelper.GetUrlResults<Setting>(url);

            return items;
        }




        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
