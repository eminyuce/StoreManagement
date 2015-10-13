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
    public class ProductAttributeRelationsController : BaseApiController<ProductAttributeRelation>, IProductAttributeRelationService
    {
        public override IEnumerable<ProductAttributeRelation> GetAll()
        {
            throw new NotImplementedException();
        }

        public override ProductAttributeRelation Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(ProductAttributeRelation value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, ProductAttributeRelation value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}
