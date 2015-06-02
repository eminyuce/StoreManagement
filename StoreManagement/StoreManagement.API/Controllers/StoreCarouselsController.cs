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
    public class StoreCarouselsController : BaseApiController<StoreCarousel>, IStoreCarouselService
    {
         
        public List<StoreCarousel> GetStoreCarousels(int storeId)
        {
            return this.StoreCarouselRepository.GetStoreCarousels(storeId);
        }

        public override IEnumerable<StoreCarousel> GetAll()
        {
            throw new NotImplementedException();
        }

        public override StoreCarousel Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(StoreCarousel value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, StoreCarousel value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
