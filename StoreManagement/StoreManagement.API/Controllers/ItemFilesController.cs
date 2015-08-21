using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class ItemFilesController : BaseApiController<ItemFile>, IItemFileService
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
