using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.API.Controllers
{
    public class ProductAttributeServicesController : BaseApiController<ProductAttribute>, IProductAttributeGeneralRepository
    {
        public override IEnumerable<ProductAttribute> GetAll()
        {
            throw new NotImplementedException();
        }

        public override ProductAttribute Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(ProductAttribute value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, ProductAttribute value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
