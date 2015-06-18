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
        private const String ApiControllerName = "Settings";
        public SettingService(string webServiceAddress)
            : base(webServiceAddress)
        {
        }

        public List<Setting> GetStoreSettings(int storeid)
        {
            
                string url = string.Format("http://{0}/api/{1}/GetStoreSettings?storeid={2}", WebServiceAddress, ApiControllerName, storeid);
                return HttpRequestHelper.GetUrlResults<Setting>(url);
            
        }

        public List<Setting> GetStoreSettingsFromCache(int storeid)
        {
             
                string url = string.Format("http://{0}/api/{1}/GetStoreSettingsFromCache?storeid={2}", WebServiceAddress, ApiControllerName, storeid);
                return HttpRequestHelper.GetUrlResults<Setting>(url);
             
        }

        public List<Setting> GetStoreSettingsByType(int storeid, string type)
        {
             
                string url = string.Format("http://{0}/api/{1}/GetStoreSettings?storeid={2}&type={3}", WebServiceAddress, ApiControllerName, storeid, type);
                return HttpRequestHelper.GetUrlResults<Setting>(url);
            
        }
    }
}
