using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiRepositories
{
    public class LocationApiRepository : BaseApiRepository, ILocationService
    {

        protected override string ApiControllerName { get { return "Locations"; } }


        public LocationApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Location>> GetLocationsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetLocationsAsync?" +
                                                 "storeId={2}" +
                                                 "&take={3}&isActive={4}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId,
                                                 take, 
                                                 isActive);

                return HttpRequestHelper.GetUrlResultsAsync<Location>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }
    }
}
