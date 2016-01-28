using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.ApiRepositories
{
    public class NavigationApiRepository : BaseApiRepository, INavigationService
    {

        protected override string ApiControllerName { get { return "Navigations"; } }

        public NavigationApiRepository(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
        public String IsConnectionOk(String webServiceAddress)
        {
            string url = string.Format("http://{0}//api/home/testconnection", webServiceAddress);
            int cacheSecond = 0;
            var response = HttpRequestHelper.MakeJsonRequest(url);
            return response;

        }
        public List<Navigation> GetStoreNavigations(int storeId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<Navigation>(url);

        }

        public List<Navigation> GetStoreActiveNavigations(int storeId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetStoreActiveNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            HttpRequestHelper.CacheMinute = ProjectAppSettings.GetWebConfigInt("RequestHelperCacheLongMinute", 600);
            return HttpRequestHelper.GetUrlResults<Navigation>(url);

        }
        public Task<List<Navigation>> GetStoreActiveNavigationsAsync(int storeId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetStoreActiveNavigationsAsync?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            HttpRequestHelper.CacheMinute = ProjectAppSettings.GetWebConfigInt("RequestHelperCacheLongMinute", 600);
            return HttpRequestHelper.GetUrlResultsAsync<Navigation>(url);
        }

       
    }
}
