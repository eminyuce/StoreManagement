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
            try
            {
                var response = HttpRequestHelper.MakeJsonRequest(url);
                return response;
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return String.Empty;
            }
        }
        public List<Navigation> GetStoreNavigations(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return HttpRequestHelper.GetUrlResults<Navigation>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Navigation>();
            }
        }

        public List<Navigation> GetStoreActiveNavigations(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetStoreActiveNavigations?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                HttpRequestHelper.CacheMinute = ProjectAppSettings.GetWebConfigInt("RequestHelperCacheLongMinute", 600);
                return HttpRequestHelper.GetUrlResults<Navigation>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Navigation>();
            }
        }
    }
}
