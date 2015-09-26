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
    public class LocationsController : BaseApiController<Location>, ILocationService
    {
        public override IEnumerable<Location> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Location Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Location value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Location value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Location>> GetLocationsAsync(int storeId, int? take, bool? isActive)
        {
            return LocationRepository.GetLocationsAsync(storeId, take, isActive);
        }
    }
}
