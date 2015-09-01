using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class StoreService : BaseService, IStoreService
    {
        protected override string ApiControllerName { get { return "Stores"; } }
        public StoreService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public Store GetStoreByDomain(string domainName)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreByDomain?domainName={2}", WebServiceAddress, ApiControllerName, domainName);
            SetCache();
            return HttpRequestHelper.GetUrlResult<Store>(url);

        }


        public Store GetStore(string domain)
        {

            string url = string.Format("http://{0}/api/{1}/GetStore?domain={2}", WebServiceAddress, ApiControllerName, domain);
            SetCache();
            return HttpRequestHelper.GetUrlResult<Store>(url);

        }


        public Store GetStore(int id)
        {
            string url = string.Format("http://{0}/api/{1}/GetSingle?id={2}", WebServiceAddress, ApiControllerName, id);
            SetCache();
            return HttpRequestHelper.GetUrlResult<Store>(url);

        }

        public Store GetStoreByUserName(string userName)
        {

            string url = string.Format("http://{0}/api/{1}/GetStoreByUserName?userName={2}", WebServiceAddress, ApiControllerName, userName);
            SetCache();
            return HttpRequestHelper.GetUrlResult<Store>(url);

        }

        public bool GetStoreCacheStatus(int id)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreCacheStatus?id={2}", WebServiceAddress, ApiControllerName, id);
            HttpRequestHelper.IsCacheEnable = false;
            return HttpRequestHelper.GetUrlResult<bool>(url);
        }

        public int GetStoreIdByDomain(string domainName)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreIdByDomain?domainName={2}", WebServiceAddress, ApiControllerName, domainName);
            SetCache();
            return HttpRequestHelper.GetUrlResult<int>(url);
        }

        public Task<Store> GetStoreAsync(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreAsync?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            SetCache();
            return HttpRequestHelper.GetUrlResultAsync<Store>(url);
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
