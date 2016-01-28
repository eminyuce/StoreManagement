using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class PageDesignApiService : BaseApiService, IPageDesignService
    {
        protected override string ApiControllerName { get { return "PageDesigns"; } }


        public PageDesignApiService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public Task<PageDesign> GetPageDesignByName(int storeId, string name)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetPageDesignByName?storeId={2}&name={3}", WebServiceAddress, ApiControllerName, storeId, name);
                var item = HttpRequestHelper.GetUrlResultAsync<PageDesign>(url);
                return item;
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Error:" + ex.StackTrace);
                return null;
            }
        }
 

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
