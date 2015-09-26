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
    public class LabelsController : BaseApiController<Label>, ILabelService
    {
        public override IEnumerable<Label> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Label Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Label value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Label value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Label>> GetLabelsByItemTypeId(int storeId, int itemId, string itemType)
        {
            return await LabelRepository.GetLabelsByItemTypeId(storeId, itemId, itemType);
        }
    }
}
