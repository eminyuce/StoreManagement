using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Services
{
    public class NavigationService : BaseService, INavigationService 
    {
        public NavigationService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public String IsConnectionOk(String webServiceAddress)
        {
            string url = string.Format("http://{0}//api/home/testconnection", webServiceAddress);
            int cacheSecond = 0;
            try
            {
                var response = RequestHelper.MakeJsonRequest(url);
                return response;
            }
            catch (Exception)
            {
                WebServiceAddress = string.Empty;
                return String.Empty;
            }
        }
        public List<Navigation> GetStoreNavigations(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/Navigations/GetNavigations?storeId={1}", WebServiceAddress, storeId);

                var responseContent = RequestHelper.GetJsonFromCacheOrWebservice(url);
                if (!String.IsNullOrEmpty(responseContent))
                {
                    String jsonString = responseContent;
                    var categories = JsonConvert.DeserializeObject<List<Navigation>>(jsonString);
                    return categories;
                }
                else
                {
                    return new List<Navigation>();
                }
            }
            catch (Exception)
            {
                WebServiceAddress = string.Empty;
                return new List<Navigation>();
            }
        }
    }
}
