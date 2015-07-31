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

namespace StoreManagement.Service.Services
{
    public class NavigationService : BaseService, INavigationService
    {
        private const String ApiControllerName = "Navigations";
        public NavigationService(string webServiceAddress)
            : base(webServiceAddress)
        {

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
            string url = string.Format("http://{0}/api/{1}/GetNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<Navigation>(url);

        }
        
        public List<Navigation> GetStoreActiveNavigations(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreActiveNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            HttpRequestHelper.CacheMinute = ProjectAppSettings.GetWebConfigInt("RequestHelperCacheLongMinute", 600);
            return HttpRequestHelper.GetUrlResults<Navigation>(url);

        }
        public Task<List<Navigation>> GetStoreActiveNavigationsAsync(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreActiveNavigationsAsync?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            HttpRequestHelper.CacheMinute = ProjectAppSettings.GetWebConfigInt("RequestHelperCacheLongMinute", 600);
            return HttpRequestHelper.GetUrlResultsAsync<Navigation>(url);
        }
    }
}
