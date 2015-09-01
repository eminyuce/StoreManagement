using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ContactService : BaseService, IContactService
    {
        public ContactService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        protected override string ApiControllerName
        {
            get { return "Contacts"; }
        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetContactsByStoreIdAsync" +
                                           "?storeId={2}" +
                                            "&take={3}&isActive={4}", WebServiceAddress, ApiControllerName, storeId, take, isActive);
                return HttpRequestHelper.GetUrlResultsAsync<Contact>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "API:GetBrandsAsync", storeId, isActive);
                return null;
            }
        }
    }
}
