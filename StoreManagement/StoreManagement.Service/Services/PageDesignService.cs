using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class PageDesignService : BaseService, IPageDesignService
    {
        private const String ApiControllerName = "PageDesigns";
        public PageDesignService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        public List<PageDesign> GetPageDesignByStoreId(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetPageDesignByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return HttpRequestHelper.GetUrlResults<PageDesign>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<PageDesign>();
            }
        }
    }
}
