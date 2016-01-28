using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class ActivityApiRepository : BaseApiRepository, IActivityGeneralRepository
    {
        public ActivityApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }

        protected override string ApiControllerName
        {
            get { return "Activities"; }
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Activity>> GetActivitiesAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetActivitiesAsync?" +
                                                 "storeId={2}" +
                                                 "&take={3}&isActive={4}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId,
                                                 take,
                                                 isActive);

                return HttpRequestHelper.GetUrlResultsAsync<Activity>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }
    }
}
