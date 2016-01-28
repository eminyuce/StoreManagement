using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiRepositories
{
    public class StoreUserApiRepository : BaseApiRepository, IStoreUserService
    {

        protected override string ApiControllerName { get { return "StoreUsers"; } }

        public StoreUserApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public StoreUser GetStoreUserByUserId(int userId)
        {
            string url = string.Format("http://{0}/api/{1}/GetStoreUserByUserId?userId={2}", WebServiceAddress, ApiControllerName, userId);
            return HttpRequestHelper.GetUrlResult<StoreUser>(url);
        }

 

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
