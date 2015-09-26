using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class StoreLanguagesController : BaseApiController<StoreLanguage>
    {
        public override IEnumerable<StoreLanguage> GetAll()
        {
            throw new NotImplementedException();
        }

        public override StoreLanguage Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(StoreLanguage value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, StoreLanguage value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
