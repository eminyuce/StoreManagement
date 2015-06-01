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
    public class StoreCarouselsController : BaseApiController, IStoreCarouselService
    {
        // GET api/storecarousels
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/storecarousels/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/storecarousels
        public void Post([FromBody]string value)
        {
        }

        // PUT api/storecarousels/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/storecarousels/5
        public void Delete(int id)
        {
        }

        public List<StoreCarousel> GetStoreCarousels(int storeId)
        {
            return this.StoreCarouselRepository.GetStoreCarousels(storeId);
        }
    }
}
