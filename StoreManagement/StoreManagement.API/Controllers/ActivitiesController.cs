using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class ActivitiesController : BaseApiController<Activity>, IActivityService
    {
        public override IEnumerable<Activity> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Activity Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Activity value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Activity value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Activity>> GetActivitiesAsync(int storeId, int? take, bool? isActive)
        {
            return await ActivityRepository.GetActivitiesAsync(storeId, take, isActive);
        }
    }
}
