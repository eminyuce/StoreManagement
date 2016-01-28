using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class ItemFilesController : BaseApiController<ItemFile>, IItemFileGeneralRepository
    {
        public override IEnumerable<ItemFile> GetAll()
        {
            throw new NotImplementedException();
        }

        public override ItemFile Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(ItemFile value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, ItemFile value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
