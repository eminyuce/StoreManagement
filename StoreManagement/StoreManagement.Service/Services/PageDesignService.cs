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
        public PageDesignService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public PageDesign GetPageDesignByName(int storeId, string name)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetPageDesignByName?storeId={2}&name={3}", WebServiceAddress, ApiControllerName, storeId, name);
                return HttpRequestHelper.GetUrlResult<PageDesign>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new PageDesign();
            }
        }
    }
}
